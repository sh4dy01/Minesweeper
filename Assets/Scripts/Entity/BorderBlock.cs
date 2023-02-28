using Managers;
using UnityEngine;


public class BorderBlock : MonoBehaviour
{
        [SerializeField] private Rigidbody2D _rigidBody;
        
        public void SetDynamic()
        {
                _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        }
}
