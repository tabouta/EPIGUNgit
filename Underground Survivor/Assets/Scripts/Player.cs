using UnityEngine;
using Mirror;
using System.Collections;
public class Player : NetworkBehaviour
{
     [SyncVar]
     private bool _isDead = false;
     public bool isDead
     {
          get {return _isDead;}
          private set {_isDead = value;}
     }
   [SerializeField]
   private float maxHealth = 100f;

   [SyncVar]
   private float currentHealth;
   [SerializeField]
   private Behaviour[] disableOnDeath;
   private bool[] wasEnabledOnStart;
   public void Setup()
   {
        SetDefaults();
   }
   public float GetHealthPercent()
   {
     return (float) currentHealth/maxHealth;
   }
   public void SetDefaults()
   {
        isDead = false;  
        currentHealth = maxHealth;
        wasEnabledOnStart = new bool [disableOnDeath.Length];
        for(int i = 0; i < disableOnDeath.Length;i++)
        {
             wasEnabledOnStart[i] = disableOnDeath[i].enabled;
        }
        Collider col = GetComponent<Collider>();
        if(col != null)
        {
             col.enabled = true;
        }
   }

     private IEnumerator Respawn()
     {
          yield return new WaitForSeconds(5f);
          SetDefaults();
          Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
          transform.position = spawnPoint.position;
          transform.rotation = spawnPoint.rotation;
     }
   [ClientRpc]
   public void RpcTakeDamage(float amount)
   {
        if(isDead)
        {
             return;
        }
       currentHealth -= amount;
       Debug.Log(transform.name + " a maintenant : " + currentHealth + " points de vie.");
       if(currentHealth <= 0)
       {
            Die();
       }
   }

   private void Die()
   {
        isDead = true;
        for(int i = 0; i < disableOnDeath.Length;i++)
        {
             disableOnDeath[i].enabled = false;
        }
         Collider col = GetComponent<Collider>();
        if(col != null)
        {
             col.enabled = false;
        }
        Debug.Log(transform .name + "à été éliminé.");
        StartCoroutine(Respawn());
   }
}
