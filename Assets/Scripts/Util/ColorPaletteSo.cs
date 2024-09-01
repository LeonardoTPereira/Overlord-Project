using UnityEngine;

namespace Util
{
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "Util/ColorPalettes", order = 0)]
    public class ColorPaletteSo : ScriptableObject
    {
        [field: SerializeField] public Color MainColorA { get; set; }
        [field: SerializeField] public Color MainColorB { get; set; }
        [field: SerializeField] public Color MainColorC { get; set; }
        [field: SerializeField] public Color MainColorD { get; set; }
        [field: SerializeField] public Color DetailColorA { get; set; }
        [field: SerializeField] public Color DetailColorB { get; set; }
        [field: SerializeField] public Color DetailColorC { get; set; }
        [field: SerializeField] public Color DetailColorD { get; set; }
        [field: SerializeField] public Color OutfitColorA { get; set; }
        [field: SerializeField] public Color OutfitColorB { get; set; }
        [field: SerializeField] public Color OutfitColorC { get; set; }
        [field: SerializeField] public Color OutfitColorD { get; set; }
    }
}