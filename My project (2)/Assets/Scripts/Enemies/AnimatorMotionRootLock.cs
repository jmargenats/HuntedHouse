using UnityEngine;

[DefaultExecutionOrder(1000)]
public sealed class AnimatorMotionRootLock : MonoBehaviour
{
    [SerializeField] private string motionRootName = "Position";

    [Header("Ajuste al moverse")]
    [SerializeField] private string movingParameterName = "isMoving";
    [SerializeField] private float movingVerticalOffset = 0f;
    [SerializeField, Min(0.1f)] private float movingOffsetSpeed = 1.5f;

    private Animator animator;
    private Transform modelRoot;
    private Transform motionRoot;
    private Transform hips;
    private Transform leftFoot;
    private Transform rightFoot;
    private SkinnedMeshRenderer[] renderers;

    private Vector3 modelRootLocalPosition;
    private Quaternion modelRootLocalRotation;
    private Vector3 motionRootLocalPosition;
    private Quaternion motionRootLocalRotation;
    private Vector3 hipsLocalPosition;
    private Vector3 rendererCenterInParent;
    private Vector3 modelRootCorrection;
    private bool hasRendererCenter;
    private bool hasMovingParameter;
    private bool isMoving;
    private bool hasFootReference;
    private float idleLowestFootHeight;
    private float currentMovingVerticalOffset;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>(true);

        if (animator == null)
        {
            enabled = false;
            return;
        }

        animator.applyRootMotion = false;

        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool &&
                parameter.name == movingParameterName)
            {
                hasMovingParameter = true;
                break;
            }
        }

        modelRoot = animator.transform;
        motionRoot = FindChildRecursive(modelRoot, motionRootName);

        modelRootLocalPosition = modelRoot.localPosition;
        modelRootLocalRotation = modelRoot.localRotation;

        if (motionRoot != null)
        {
            motionRootLocalPosition = motionRoot.localPosition;
            motionRootLocalRotation = motionRoot.localRotation;
        }

        if (animator.isHuman && animator.avatar != null && animator.avatar.isValid)
        {
            hips = animator.GetBoneTransform(HumanBodyBones.Hips);

            if (hips != null)
                hipsLocalPosition = hips.localPosition;

            leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            hasFootReference = TryGetLowestFootHeight(out idleLowestFootHeight);
        }

        renderers = animator.GetComponentsInChildren<SkinnedMeshRenderer>(true);

        hasRendererCenter = TryGetRendererCenter(out Vector3 center);

        if (hasRendererCenter)
            rendererCenterInParent = transform.InverseTransformPoint(center);
    }

    private void LateUpdate()
    {
        if (animator == null || !animator.enabled)
            return;

        UpdateMovingVerticalOffset();

        Vector3 visualMovingOffset =
            Vector3.up * currentMovingVerticalOffset;

        // El NavMeshAgent mueve el padre. La animacion nunca debe desplazar
        // el modelo completo fuera de ese padre.
        modelRoot.localPosition =
            modelRootLocalPosition + modelRootCorrection + visualMovingOffset;
        modelRoot.localRotation = modelRootLocalRotation;

        if (motionRoot != null)
        {
            motionRoot.localPosition = motionRootLocalPosition;
            motionRoot.localRotation = motionRootLocalRotation;
        }

        // Algunos FBX de Mixamo guardan el salto horizontal en la cadera
        // aunque Root Transform Position XZ este horneado.
        if (isMoving && hips != null)
        {
            hips.localPosition = hipsLocalPosition;
        }

        if (isMoving)
            CorrectLargeAnimationOffset(visualMovingOffset);

        ApplyFootGrounding();
    }

    private void UpdateMovingVerticalOffset()
    {
        isMoving =
            hasMovingParameter && animator.GetBool(movingParameterName);

        float targetOffset = isMoving ? movingVerticalOffset : 0f;

        currentMovingVerticalOffset = Mathf.MoveTowards(
            currentMovingVerticalOffset,
            targetOffset,
            movingOffsetSpeed * Time.deltaTime
        );
    }

    private void CorrectLargeAnimationOffset(Vector3 visualMovingOffset)
    {
        if (!hasRendererCenter || !TryGetRendererCenter(out Vector3 center))
            return;

        Vector3 currentCenter = transform.InverseTransformPoint(center);
        Vector3 centerWithoutCorrection =
            currentCenter - modelRootCorrection - visualMovingOffset;
        Vector3 offset = centerWithoutCorrection - rendererCenterInParent;

        bool invalidHorizontalOffset =
            new Vector2(offset.x, offset.z).magnitude > 0.75f;
        bool invalidVerticalOffset = Mathf.Abs(offset.y) > 0.5f;

        modelRootCorrection =
            invalidHorizontalOffset || invalidVerticalOffset
                ? -offset
                : Vector3.zero;

        modelRoot.localPosition =
            modelRootLocalPosition + modelRootCorrection + visualMovingOffset;
    }

    private void ApplyFootGrounding()
    {
        if (!isMoving || !hasFootReference ||
            !TryGetLowestFootHeight(out float currentLowestFootHeight))
        {
            return;
        }

        float targetFootHeight = idleLowestFootHeight;
        float verticalCorrection = targetFootHeight - currentLowestFootHeight;

        modelRoot.localPosition += Vector3.up * verticalCorrection;
    }

    private bool TryGetLowestFootHeight(out float lowestFootHeight)
    {
        lowestFootHeight = float.PositiveInfinity;
        bool foundFoot = false;

        if (leftFoot != null)
        {
            lowestFootHeight =
                transform.InverseTransformPoint(leftFoot.position).y;
            foundFoot = true;
        }

        if (rightFoot != null)
        {
            float rightFootHeight =
                transform.InverseTransformPoint(rightFoot.position).y;
            lowestFootHeight = Mathf.Min(lowestFootHeight, rightFootHeight);
            foundFoot = true;
        }

        return foundFoot;
    }

    private bool TryGetRendererCenter(out Vector3 center)
    {
        center = Vector3.zero;

        if (renderers == null || renderers.Length == 0)
            return false;

        bool foundRenderer = false;
        Bounds combinedBounds = default;

        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            if (renderer == null || !renderer.enabled)
                continue;

            if (!foundRenderer)
            {
                combinedBounds = renderer.bounds;
                foundRenderer = true;
            }
            else
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }
        }

        if (!foundRenderer)
            return false;

        center = combinedBounds.center;
        return true;
    }

    private static Transform FindChildRecursive(Transform parent, string childName)
    {
        if (parent.name == childName)
            return parent;

        foreach (Transform child in parent)
        {
            Transform result = FindChildRecursive(child, childName);

            if (result != null)
                return result;
        }

        return null;
    }
}
