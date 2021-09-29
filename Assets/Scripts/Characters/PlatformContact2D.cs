using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    

    public readonly struct PlatformContact2D 
    {
        public readonly Collider2D Collider;
        public readonly Rigidbody2D Rigidbody;
        public readonly bool HasRigidbody;
        public readonly Vector2 Point;

        public PlatformContact2D(Collider2D collider, Rigidbody2D rigidbody, Vector2 point)
        {
            Collider = collider;
            Rigidbody = rigidbody;
            HasRigidbody = rigidbody != null;
            Point = HasRigidbody ? rigidbody.GetPoint(point) : point;
        }
        
        public PlatformContact2D(RaycastHit2D hit) : this(hit.collider, hit.collider.attachedRigidbody, hit.point) { }

        public static implicit operator PlatformContact2D(RaycastHit2D hit) => new PlatformContact2D(hit);
        
        
        public PlatformContact2D(ContactPoint2D contact) : this(contact.collider, contact.collider.attachedRigidbody, contact.point) { }

        public static implicit operator PlatformContact2D(ContactPoint2D hit) => new PlatformContact2D(hit);

        
        public Vector2 Velocity => HasRigidbody ? Rigidbody.GetRelativePointVelocity(Point) : default;

    }
}