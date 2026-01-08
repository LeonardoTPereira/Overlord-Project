using System;
using System.Collections.Generic;
using UnityEngine;
using Util;
using ScriptableObjects;

// TODO: Esse script é uma "gambiarra" e deve ser destruida,
// apos as pastas de cada modulo (Game, TopdownGame, PlatformGame, Overlord, Utils)
// conterem todos os scripts do projeto. Scripts como WeaponTypeRuntimeSetSO devem ser destruidos tbm
// Ele basicamente "converte" os valores de Movement e Weapon SO novos para os antigos, 
// que sao usados nos modulos de jogo. Como o objetivo primario é retirar delimitar o modulo OVERLORD,
// foco em focar nele antes de remover scripts antigos que convertem o OUTPUT do Overlord para dados de jogo específicos
public class OverlordDataToGameDataConverter: MonoBehaviour
{
    [SerializeField] public WeaponTypeRuntimeSetSO WeaponSet;

    public static List<MovementTypeSO> ToMovementTypeSOList(EnemyMovementsSOInterface movementSet)
    {
        var result = new List<MovementTypeSO>();

        if (movementSet == null)
            return result;

        var allMovements = movementSet.GetAllMovementEnums();

        foreach (var movement in allMovements)
        {
            MovementTypeSO so = ScriptableObject.CreateInstance<MovementTypeSO>();
            so.enemyMovementIndex = ConvertToMovementEnum(movement);
            result.Add(so);
        }

        return result;
    }

    private static Enums.MovementEnum ConvertToMovementEnum(Enum movement)
    {
        try
        {
            return (Enums.MovementEnum)Enum.Parse(typeof(Enums.MovementEnum), movement.ToString());
        }
        catch
        {
            Debug.LogWarning($"[EnemyMovementConverter] Enum {movement} não pôde ser convertido para Enums.MovementEnum.");
            return default;
        }
    }
}
