public interface IPickupable
{
    bool CanPickup();

    void Pickup();
}

public interface IInteractable
{
    void Interact();
}