using UnityEngine;

public class EnemySpriteResolver : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void ApplySprite(
        SpaceShooterMovement movementType,
        SpaceShooterWeapon weaponType)
    {
        string spriteName = $"inimigo0{weaponType.index}{movementType.index}";

        Sprite sprite = Resources.Load<Sprite>(
            $"SpaceShooterSprites/Enemies/{spriteName}");

        if (sprite == null)
        {
            Debug.LogError($"Sprite não encontrado: {spriteName}");
            return;
        }

        spriteRenderer.sprite = sprite;
    }
}
