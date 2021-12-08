using UnityEngine;
using Mirror;
public class PlayerSetup : NetworkBehaviour
{
   [SerializeField]
   Behaviour[] componentsToDisable;
   [SerializeField]
   private string remoteLayerName = "RemotePlayer";

   private Camera sceneCamera;
   private void Start()
   {
      if (!isLocalPlayer)
      {
         DisableComponent();
         AssignRemoteLayer();
      }
      else
      {
         sceneCamera = Camera.main;
         if (sceneCamera != null)
         {
            sceneCamera.gameObject.SetActive(false);
         }
      }
      
   }

   public override void OnStartClient()
   {
      base.OnStartClient();
      string netId = GetComponent<NetworkIdentity>().netId.ToString();
      Player player = GetComponent<Player>();
      GameManager.RegisterPlayer(netId,player);
   }
   private void AssignRemoteLayer()
   {
      gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
   }
   private void DisableComponent() 
   {
    	//On va faire une boucle afin de savoir quel composants désactiver si le joueur n'est pas le notre
         for (int i = 0; i < componentsToDisable.Length; i++)
         {
            componentsToDisable[i].enabled = false;
         }
   }

   private void OnDisable()
   {
      if (sceneCamera != null)
      {
         sceneCamera.gameObject.SetActive(true);
      }
      GameManager.UnregisterPlayer(transform.name); 
   }
}
