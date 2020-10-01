using EnemyGenerator;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;

#if UNITY_EDITOR
public class TimSort : MonoBehaviour
{
    public const int RUN = 32;

    // this function sorts array from left index to 
    // to right index which is of size atmost RUN 
    public static void insertionSort(ref NativeArray<EnemyComponent> enemies, ref NativeArray<WeaponComponent> weapons, int left, int right)
    {
        for (int i = left + 1; i <= right; i++)
        {
            EnemyComponent auxEnemy;
            WeaponComponent auxWeapon;
            auxEnemy = enemies[i];
            auxWeapon = weapons[i];

            int j = (i - 1);
            while ((j >= left) && (math.abs(EnemyUtil.desiredFitness - enemies[j].fitness) > math.abs(EnemyUtil.desiredFitness - auxEnemy.fitness)))
            {
                //Debug.Log("j = " + j);
                enemies[j + 1] = enemies[j];
                weapons[j + 1] = weapons[j];

                j--;
            }
            enemies[j + 1] = auxEnemy;
            weapons[j + 1] = auxWeapon;
        }
    }

    // merge function merges the sorted runs 
    public static void merge(ref NativeArray<EnemyComponent> enemies, ref NativeArray<WeaponComponent> weapons, int l, int m, int r)
    {
        // original array is broken in two parts 
        // left and right array 
        int len1 = m - l + 1, len2 = r - m;

        if (len2 < 0)
            len2 = 0;
        if (len1 < 0)
            len1 = 0;

        //Debug.Log("r = " + r + "- m = " + m + "- len2= " + len2);
        NativeArray<EnemyComponent> leftEnemies = new NativeArray<EnemyComponent>(len1, Allocator.Temp);
        NativeArray<WeaponComponent> leftWeapons = new NativeArray<WeaponComponent>(len1, Allocator.Temp);

        NativeArray<EnemyComponent> rightEnemies = new NativeArray<EnemyComponent>(len2, Allocator.Temp);
        NativeArray<WeaponComponent> rightWeapons = new NativeArray<WeaponComponent>(len2, Allocator.Temp);

        for (int x = 0; x < len1; x++)
        {
            leftEnemies[x] = enemies[l + x];
            leftWeapons[x] = weapons[l + x];
        }
        for (int x = 0; x < len2; x++)
        {
            rightEnemies[x] = enemies[m + 1 + x];
            rightWeapons[x] = weapons[m + 1 + x];
        }

        int i = 0;
        int j = 0;
        int k = l;

        // after comparing, we merge those two array 
        // in larger sub array 
        while (i < len1 && j < len2)
        {
            if (math.abs(EnemyUtil.desiredFitness - leftEnemies[i].fitness) <= math.abs(EnemyUtil.desiredFitness - rightEnemies[j].fitness))
            {
                weapons[k] = leftWeapons[i];
                enemies[k] = leftEnemies[i];
                i++;
            }
            else
            {
                weapons[k] = rightWeapons[j];
                enemies[k] = rightEnemies[j];
                j++;
            }
            k++;
        }

        // copy remaining elements of left, if any 
        while (i < len1)
        {
            weapons[k] = leftWeapons[i];
            enemies[k] = leftEnemies[i];
            k++;
            i++;
        }

        // copy remaining element of right, if any 
        while (j < len2)
        {
            weapons[k] = rightWeapons[j];
            enemies[k] = rightEnemies[j];
            k++;
            j++;
        }

        /*leftFitness.Dispose();
        leftEnemies.Dispose();
        leftWeapons.Dispose();
        rightFitness.Dispose();
        rightEnemies.Dispose();
        rightWeapons.Dispose();*/
    }

    // iterative Timsort function to sort the 
    // array[0...n-1] (similar to merge sort) 
    public static void timSort(ref NativeArray<EnemyComponent> enemies, ref NativeArray<WeaponComponent> weapons, int n)
    {
        // Sort individual subarrays of size RUN 
        for (int i = 0; i < n; i += RUN)
        {
            insertionSort(ref enemies, ref weapons, i, Mathf.Min((i + 31), (n - 1)));
        }

        // start merging from size RUN (or 32). It will merge 
        // to form size 64, then 128, 256 and so on .... 
        for (int size = RUN; size < n; size = 2 * size)
        {
            // pick starting point of left sub array. We 
            // are going to merge arr[left..left+size-1] 
            // and arr[left+size, left+2*size-1] 
            // After every merge, we increase left by 2*size 
            for (int left = 0; left < n; left += 2 * size)
            {
                // find ending point of left sub array 
                // mid+1 is starting point of right sub array 
                int mid = Mathf.Min((left + size - 1), n - 1);
                int right = Mathf.Min((left + 2 * size - 1), (n - 1));

                // merge sub array arr[left.....mid] & 
                // arr[mid+1....right] 
                merge(ref enemies, ref weapons, left, mid, right);
            }
        }
    }


    //This code is contributed by DrRoot_ 
}

public class TimSortSystem : JobComponentSystem
{
    //[BurstCompile]
    public struct SortEnemiesJob : IJob
    {
        public NativeArray<EnemyComponent> enemies;
        public NativeArray<WeaponComponent> weapons;

        public void Execute()
        {
            //Debug.Log(enemies.Length + " - " + weapons.Length + " - " + fitness.Length);
            TimSort.timSort(ref enemies, ref weapons, enemies.Length);


            /*for (var i = 0; i <= enemies.Length - 2; ++i)
            {
                var j = random.NextInt(i, enemies.Length);
                var tmp = enemies[i];
                enemies[i] = enemies[j];
                enemies[j] = tmp;
            }*/
        }
    }

    [BurstCompile]
    struct EmptyJob : IJob
    {
        public void Execute()
        {
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle handle;
        if (GameManager.instance.createEnemy)
        {
            if (GameManagerTest.instance.enemyReady && !GameManagerTest.instance.enemySorted)
            {
                SortEnemiesJob sortJob = new SortEnemiesJob
                {
                    enemies = GameManagerTest.instance.enemyPop,
                    weapons = GameManagerTest.instance.weaponPop,
                };
                //return job.Schedule(this, inputDeps);

                handle = sortJob.Schedule(inputDeps);
                handle.Complete();

                /*for (int i = 0; i < GameManagerTest.instance.enemyPop.Length; ++i)
                    Debug.Log("i = " + i + " - Fitness = " + GameManagerTest.instance.enemyPop[i].fitness + "Fitness - " + GameManagerTest.instance.fitnessArray[i]);*/

                GameManagerTest.instance.enemySorted = true;

                return handle;
            }
        }
        EmptyJob emptyJob = new EmptyJob
        { };
        handle = emptyJob.Schedule();
        return handle;
    }
}
#endif