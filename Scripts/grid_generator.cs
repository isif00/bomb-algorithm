using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid_generator : MonoBehaviour
{
    [SerializeField] GameObject gridPrefab;
    [SerializeField] GameObject enemy, bomb, bomb2;
    [SerializeField] float X, Y;
    [SerializeField] int enemies_amount, radius;

    float heigth, length, distance;
    Vector3 best_position, best_position2;
    List<Vector3> enemies_positions = new List<Vector3>();

    void Start() {
        bomb.transform.localScale = new Vector3(radius, radius, 0);
        bomb2.transform.localScale = new Vector3(radius, radius, 0);

        length = gridPrefab.transform.localScale.x; 
        heigth = gridPrefab.transform.localScale.y;

        for (int i = 0; i < X; i++) {
            for (int j = 0; j < Y; j++) {
                GameObject grid = Instantiate(gridPrefab) as GameObject;
                grid.transform.position = new Vector3(i, j, 0);
            }
        }

        for (int i = 0; i < enemies_amount; i++){
            var enemy_position = new Vector3(Random.Range(0, X-(length/2)), Random.Range(0, Y-(heigth/2)), 1);
            enemies_positions.Add(enemy_position);
            Instantiate(enemy, enemy_position, Quaternion.identity);
        }

        best_position = FindTheBestPosition(enemies_positions, enemies_amount, radius);
        Instantiate(bomb, best_position, Quaternion.identity);

        best_position2 = FindTheBestPosition2(X, Y, enemies_positions, enemies_amount, radius);
        Instantiate(bomb2, best_position2, Quaternion.identity);
    }

    float calculate_distance(Vector3 pos1, Vector3 pos2) { 
        distance = Mathf.Sqrt(Mathf.Pow((pos1.x - pos2.x), 2) + Mathf.Pow((pos1.y - pos2.y), 2));
        return distance;
    }

    Vector3 FindTheBestPosition(List<Vector3> enemies_positions, int enemies_amount, int radius){
        float center_x = 0, center_y = 0, accuracy = 0, maxAccuracy = 0 ;
        int hits = 0, maxHits = 0; 
        Vector3 best_position = Vector3.zero; 
        
        for (int i = 0; i < enemies_amount; i++) {
            center_x = center_x + enemies_positions[i].x;
            center_y = center_y + enemies_positions[i].y;
        }

        var center = new Vector3(center_x/2, center_y/2, 0);

        for (int i = 0; i < enemies_amount; i++) {
            for (int j = 0; j < enemies_amount; j++) {
                if (calculate_distance(enemies_positions[i], enemies_positions[j]) <= radius){
                    hits++;
                }
                    
            }

            accuracy = (hits / enemies_amount)*100;
            if (accuracy > maxAccuracy) {
                maxAccuracy = accuracy;
                best_position = enemies_positions[i];
            }
        }
        Debug.Log(maxHits);
        Debug.Log(maxAccuracy);
        return best_position;
    }

    Vector3 FindTheBestPosition2(float X, float Y, List<Vector3> enemies_positions, int enemies_amount, int radius){
        Vector3 best_position = Vector3.zero; 
        float maxAccuracy = 0, accuracy = 0;
        int hits, maxHits = 0;

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                hits = 0;
                Vector3 point = new Vector3(i, j, 0);
                for (int k = 0; k < enemies_amount; k++)
                {
                    if (calculate_distance(point, enemies_positions[k]) <= radius)
                    {
                        hits++;
                    }
                }

                accuracy = (hits / enemies_amount)*100;
                if (accuracy > maxAccuracy) {
                    maxHits = hits;
                    maxAccuracy = accuracy;
                    best_position = point;
            }
            }
        
        }
        
        Debug.Log(maxHits);
        Debug.Log(maxAccuracy);
        return best_position;
    }
}
