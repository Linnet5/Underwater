using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityGeneration : MonoBehaviour
{
    [SerializeField] private GameObject groundTile;
    [SerializeField] private GameObject fishes;
    private GameObject[] tiles;
    private GameObject player;


    private int distance;
    void Start()
    {
        tiles = new GameObject[9];
        player = GameObject.FindGameObjectWithTag("Player");
        distance = groundTile.GetComponent<MeshGenerator>().resolution;

        tiles = Generate(player.transform.position, true);
    }

    void Update()
    {
        // Check forward, backward, left and right of the player. If no ground exists there, then generate new ground and delete previous ground.
        if(!Physics.CheckSphere(new Vector3(player.transform.position.x, 0, player.transform.position.z) + player.transform.forward*distance , 10.0f)) {
            Ungenerate(tiles);
            tiles = Generate(player.transform.position, false);
        }
        else if(!Physics.CheckSphere(new Vector3(player.transform.position.x, 0, player.transform.position.z) - player.transform.forward*distance , 10.0f)) {
            Ungenerate(tiles);
            tiles = Generate(player.transform.position, false);
        }
        else if(!Physics.CheckSphere(new Vector3(player.transform.position.x, 0, player.transform.position.z) - player.transform.right*distance , 10.0f)) {
            Ungenerate(tiles);
            tiles = Generate(player.transform.position, false);
        }
        else if(!Physics.CheckSphere(new Vector3(player.transform.position.x, 0, player.transform.position.z) + player.transform.right*distance , 10.0f)) {
            Ungenerate(tiles);
            tiles = Generate(player.transform.position, false);
        }
    }

    void OnDrawGizmos() {
        if(player) {
            Gizmos.DrawSphere(player.transform.position + player.transform.forward*distance, 10.0f);
            Gizmos.DrawSphere(player.transform.position - player.transform.forward*distance, 10.0f);
            Gizmos.DrawSphere(player.transform.position + player.transform.right*distance, 10.0f);
            Gizmos.DrawSphere(player.transform.position - player.transform.right*distance, 10.0f);
        }
    }

    GameObject[] Generate(Vector3 playerPosition, bool firstTime) {
        // Generates the tile under player and all adjacent tiles.
        for(int i = 0; i < 9; i++) {
            tiles[i] = GameObject.Instantiate(groundTile);
            tiles[i].tag = "Ground";

            Vector3 tilesPos = new Vector3(playerPosition.x -distance/2.0f, 0, playerPosition.z -distance/2.0f);
            
            switch(i){
                case 0:
                    tiles[i].transform.position = tilesPos;
                    SpawnFish(tiles[i], playerPosition);
                break;
                case 1:
                    tiles[i].transform.position = tilesPos + (new Vector3(distance, 0 , 0));
                    SpawnFish(tiles[i], playerPosition);
                break;
                case 2:
                    tiles[i].transform.position = tilesPos + (new Vector3(distance, 0 , distance));
                    SpawnFish(tiles[i], playerPosition);
                break;
                case 3:
                    tiles[i].transform.position = tilesPos + (new Vector3(0, 0 , distance));
                    SpawnFish(tiles[i], playerPosition);
                break;
                case 4:
                    tiles[i].transform.position = tilesPos + (new Vector3(-distance, 0 , distance));
                    SpawnFish(tiles[i], playerPosition);
                break;
                case 5:
                    tiles[i].transform.position = tilesPos + (new Vector3(-distance, 0 , 0));
                    SpawnFish(tiles[i], playerPosition);
                break;
                case 6:
                    tiles[i].transform.position = tilesPos + (new Vector3(-distance, 0 , -distance));
                    SpawnFish(tiles[i], playerPosition);
                break;
                case 7:
                    tiles[i].transform.position = tilesPos + (new Vector3(0, 0 , -distance));
                    SpawnFish(tiles[i], playerPosition);
                break;
                case 8:
                    tiles[i].transform.position = tilesPos + (new Vector3(distance, 0 , -distance));
                    SpawnFish(tiles[i], playerPosition);
                break;

                default:
                break;
            }

            tiles[i].GetComponent<MeshGenerator>().Generate(firstTime, tiles[i]);
        }
        return(tiles);
    }

    void SpawnFish(GameObject inTile, Vector3 playerPosition) {
        GameObject school = GameObject.Instantiate(fishes);
        if (school.GetComponent<Renderer>().isVisible) {
            Destroy(school);
            return;
        }
        school.transform.position = inTile.transform.position + new Vector3(0.0f, 10.0f, 0.0f);
        school.transform.LookAt(playerPosition);
    }

    void Ungenerate(GameObject[] tilesToDelete) {
        for(int i = 0; i < 9; i++) {
            Destroy(tilesToDelete[i]);
        }
    }
}
