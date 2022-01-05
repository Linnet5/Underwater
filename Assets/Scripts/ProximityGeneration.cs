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

        tiles = Generate(player.transform.position);
    }

    void FixedUpdate()
    {
        // Check forward, backward, left and right of the player. If no ground exists there, then generate new ground and delete previous ground.
        if(!Physics.CheckSphere(new Vector3(player.transform.position.x, 0, player.transform.position.z) + player.transform.forward*distance , 10.0f)) {
            Ungenerate(tiles);
            tiles = Generate(player.transform.position);
        }
        else if(!Physics.CheckSphere(new Vector3(player.transform.position.x, 0, player.transform.position.z) - player.transform.forward*distance , 10.0f)) {
            Ungenerate(tiles);
            tiles = Generate(player.transform.position);
        }
        else if(!Physics.CheckSphere(new Vector3(player.transform.position.x, 0, player.transform.position.z) - player.transform.right*distance , 10.0f)) {
            Ungenerate(tiles);
            tiles = Generate(player.transform.position);
        }
        else if(!Physics.CheckSphere(new Vector3(player.transform.position.x, 0, player.transform.position.z) + player.transform.right*distance , 10.0f)) {
            Ungenerate(tiles);
            tiles = Generate(player.transform.position);
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

    GameObject[] Generate(Vector3 playerPosition) {
        // Generates the tile under player and all adjacent tiles.
        for(int i = 0; i < 9; i++) {
            tiles[i] = GameObject.Instantiate(groundTile);
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

            tiles[i].GetComponent<MeshGenerator>().Generate();
        }
        return(tiles);
    }

    void Ungenerate(GameObject[] tilesToDelete) {
        for(int i = 0; i < 9; i++) {
            tilesToDelete[i].GetComponent<DecorPlacement>().DestroyDecor();
            Destroy(tilesToDelete[i]);
        }
    }
}
