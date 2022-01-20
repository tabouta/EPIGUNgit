using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthbarFill;
    private Player player;
    private PlayerController controller;
    public void SetPlayer(Player _player)
    {
        player = _player;
        controller = player.GetComponent<PlayerController>();
    }
    private void Update()
    {
        SetHealthAmount(player.GetHealthPercent());
    }
    void SetHealthAmount(float _amount)
    {
        healthbarFill.localScale = new Vector3(1f, _amount,1f);
    }
}
