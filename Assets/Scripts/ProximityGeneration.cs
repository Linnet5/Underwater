using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityGeneration : MonoBehaviour
{
    [SerializeField] private GameObject groundTile;
    private GameObject[] tiles;
    private GameObject player;
    // Start is called before the first frame update

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
                break;
                case 1:
                    tiles[i].transform.position = tilesPos + (new Vector3(distance, 0 , 0));
                break;
                case 2:
                    tiles[i].transform.position = tilesPos + (new Vector3(distance, 0 , distance));
                break;
                case 3:
                    tiles[i].transform.position = tilesPos + (new Vector3(0, 0 , distance));
                break;
                case 4:
                    tiles[i].transform.position = tilesPos + (new Vector3(-distance, 0 , distance));
                break;
                case 5:
                    tiles[i].transform.position = tilesPos + (new Vector3(-distance, 0 , 0));
                break;
                case 6:
                    tiles[i].transform.position = tilesPos + (new Vector3(-distance, 0 , -distance));
                break;
                case 7:
                    tiles[i].transform.position = tilesPos + (new Vector3(0, 0 , -distance));
                break;
                case 8:
                    tiles[i].transform.position = tilesPos + (new Vector3(distance, 0 , -distance));
                break;

                default:
                break;
            }

            tiles[i].GetComponent<MeshGenerator>().Generate(firstTime);
        }
        return(tiles);
    }

    void Ungenerate(GameObject[] tilesToDelete) {
        Vector3 playerPosition = player.transform.position;
        for(int i = 0; i < 9; i++) {
            tilesToDelete[i].GetComponent<DecorPlacement>().DestroyDecor(playerPosition);
            Destroy(tilesToDelete[i]);
        }
    }
}
