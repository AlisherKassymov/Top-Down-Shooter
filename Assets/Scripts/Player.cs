using UnityEngine;

public class Player : MonoBehaviour
{
   public PlayerControls PlayerControls { get; private set; }

   private void Awake()
   {
      PlayerControls = new PlayerControls();
   }
   
   private void OnEnable()
   {
      PlayerControls.Enable();
   }

   private void OnDisable()
   {
      PlayerControls.Disable();
   }

}
