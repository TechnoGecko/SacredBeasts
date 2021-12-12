
using UnityEngine;

namespace Characters
{
    public readonly struct PlatformContact
    {
        public readonly Collider2D Collider;
        public readonly Rigidbody2D Rigidbody;
        public readonly bool HasRigidbody;
        public readonly Vector2 Point;

        public PlatformContact(Collider2D collider, Rigidbody2D rigidbody, Vector2 point)
        {
            Collider = collider;
            Rigidbody = rigidbody;
            HasRigidbody = rigidbody != null;
            Point = HasRigidbody ? rigidbody.GetPoint(point) : point;
        }
        
        public PlatformContact(RaycastHit2D hit) : this(hit.collider, hit.collider.attachedRigidbody, hit.point) { }

        public static implicit operator PlatformContact(RaycastHit2D hit) => new PlatformContact(hit);
        
        // =====================================================================================================/
        
        public PlatformContact(ContactPoint2D contact) : this(contact.collider, contact.collider.attachedRigidbody, contact.point) { }
        public static implicit operator PlatformContact(ContactPoint2D hit) => new PlatformContact(hit);
        
        // =====================================================================================================/

        public Vector2 Velocity => HasRigidbody ? Rigidbody.GetRelativePointVelocity(Point) : default;
    }
}
