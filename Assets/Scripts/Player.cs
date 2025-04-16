using Controls;
using UnityEngine;

public class Player : MonoBehaviour
{
   public PlayerControls PlayerControls { get; private set; }
   public PlayerAim PlayerAim { get; private set; }

   private void Awake()
   {
      PlayerControls = new PlayerControls();
      PlayerAim = GetComponent<PlayerAim>();
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
