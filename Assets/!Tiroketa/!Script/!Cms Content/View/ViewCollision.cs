using _Tiroketa._Script._Cms_Content.Components;
using Game._Script.Interaction;
using System;
using UnityEngine;

namespace _Tiroketa._Script._Cms_Content.View
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody2D))]
    public class ViewCollision : BaseView
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            var viewCollision = other.collider.gameObject.GetComponent<ViewCollision>();
            if (viewCollision == null)
                return;

            GetModel().GetComponent<ColliderComponent>().OtherCollisionView = viewCollision;
            foreach (var interaction in InteractionCache<IColliderInteraction>.AllInteraction)
            {
                interaction.EnterCollider(GetComponent<ViewCollision>(), viewCollision);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var viewCollision = other.gameObject.GetComponent<ViewCollision>();
            if (viewCollision == null)
                return;

            GetModel().GetComponent<ColliderComponent>().OtherCollisionView = viewCollision;
            foreach (var interaction in InteractionCache<IColliderInteraction>.AllInteraction)
            {
                interaction.EnterCollider(GetComponent<ViewCollision>(), viewCollision);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var viewCollision = other.gameObject.GetComponent<ViewCollision>();
            if (viewCollision == null)
                return;

            GetModel().GetComponent<ColliderComponent>().OtherCollisionView = null;
            foreach (var interaction in InteractionCache<IColliderInteraction>.AllInteraction)
            {
                interaction.ExitCollider(GetComponent<ViewCollision>(), viewCollision);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            var viewCollision = other.collider.gameObject.GetComponent<ViewCollision>();
            if (viewCollision == null)
                return;

            GetModel().GetComponent<ColliderComponent>().OtherCollisionView = null;
            foreach (var interaction in InteractionCache<IColliderInteraction>.AllInteraction)
            {
                interaction.ExitCollider(GetComponent<ViewCollision>(), viewCollision);
            }
        }
    }
}
