using System;
using System.Text;
using UnityEngine;
using Fog.Dialogue;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ReadableItemSO", menuName = "Items/Readable Item"), Serializable]
    public class ReadableItemSo : ItemSo, IDialogueObjSo
    {
        [SerializeField] private NpcDialogueData dialogueData;

        public NpcDialogueData DialogueData
        {
            get => dialogueData;
            set => dialogueData = value;
        }

        public string GetScrollSpriteString()
        {
            var stringBuilder = new StringBuilder();
            var itemNum = 0;
            stringBuilder.Append($"<sprite=\"Scrolls\" name=\"{ItemName}\">");
            return stringBuilder.ToString();
        }

        public string SetRandomText(bool isInPortuguese)
        {
            int text = UnityEngine.Random.Range(0, 11);

            if (isInPortuguese)
            {
                switch (text)
                {
                    case 0:
                        return "Ningu�m sabe por que Valentine est� t�o mal-humorado, mas alguns dizem foi por causa do que aconteceu no �ltimo Dia dos Namorados.";
                    case 1:
                        return "[...] Dizem que ainda est�o procurando uma cura para a maldi��o que tirou seus dias de calma [...]";
                    case 2:
                        return "A Porta Perdida de Alas - Uma Hist�ria Completa";
                    case 3:
                        return "As lendas dizem que Alasdoor j� foi apenas uma porta muito grande.";
                    case 4:
                        return "Voc� achou que isso seria uma parte importante da hist�ria? Achou errado, ordin�rio!"; // Culture shock's reference
                    case 5:
                        return "O tri�ngulo que voc� procura, voc� sabe o que ele significa?";
                    case 6:
                        return "A feiticeira respons�vel pelo feiti�o est� escondida em algum lugar da floresta.";
                    case 7:
                        return "Ningu�m conhece seu nome real, apenas que ela � uma bruxa poderosa.";
                    case 8:
                        return "INGREDIENTES: FARINHA ENRIQUECIDA, �LEO DE PALMA, SAL, FLOCOS DE CENOURA DESIDRATADA, CONT�M MENOS DE 2% DE EXTRATO DE LEVEDURA AUTOLISADO, �CIDO C�TRICO, SUCO CONCENTRADO DE REPOLHO VERDE E L�TEX NATURAL.";
                    case 9:
                        return "As paredes parecem ter se fechado atr�s de mim. Como entrei neste lugar?";
                    case 10:
                        return "A receita secreta pode estar escondida em algum lugar deste calabou�o. Eu prometi ao meu pai que a encontraria... mas nem consigo ver a sa�da.";
                    default:
                        return "Acho que voc� n�o deveria estar lendo isso.";
                }
            }
            else
            {
                switch (text)
                {
                    case 0:
                        return "No one knows why Valentine is so grumpy, but some say it all happened on Valentine's Day.";
                    case 1:
                        return "[...] It is said they are still looking for a cure for the enchantment that took their calm days away [...]";
                    case 2:
                        return "The Missing Door Of Alas - A Full Story";
                    case 3:
                        return "Legends say Alasdoor was once just a very big door.";
                    case 4:
                        return "You thought this would be an interesting piece of lore, but it is I, a Jojo reference.";
                    case 5:
                        return "The triangle you are looking for, do you know what it means?";
                    case 6:
                        return "The enchantress responsible for the spell is hidden somewhere in the woods";
                    case 7:
                        return "No one knows her real name, just that she is a powerfull witch";
                    case 8:
                        return "INGREDIENTS: ENRICHED FLOUR, PALM OIL, SALT, DRIED CARROT FLAKE, CONTAINS LESS THAN 2% OF AUTOLYZED YEAST EXTRACT, CITRIC ACID, CONCENTRATED GREEN CABBAGE JUICE AND NATURAL LATEX";
                    case 9:
                        return "The walls seem to have closed behind me. How did I enter this place?";
                    case 10:
                        return "The secret recipie might be hidden in this dungeon somewhere. I have sworn my father I would find it... but I can't even see the exit.";
                    default:
                        return "I don't think you are supposed to be reading this";
                }
            }
        }
    }
}
