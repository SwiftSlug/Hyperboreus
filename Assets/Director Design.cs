using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorDesign : MonoBehaviour {


	void Start () {

        //  Set references to player


        //  Iniitalise enemy AI List

        //  Iniitalise player list

        //  Iniitalise spawn location list

    }


    void Update () {
		
        //  If cooldown expired
            
            //  Update player intensity
            

            //  If max number of enemies has not been reached

                //  Scan for spawn areas

                //  If is daytime

                    //  Spawn day enemies

                //  If is night time

                    //  Spawn night enemies

	}


    void UpdatePlayerIntensity()
    {

        //  For each player in the game

            //  Find all enemies near the player

                //  Increase nearby enemy count

            //  For each enemy within the map
                
                //  If AI is tracking player
                        
                    //  Increase tracking AI count for player

            //  Calculate finial intensity level from collected values
            

    }
    

    void ScanForSpawnAreas()
    {

        //  For the number of spawn areas needed

            //  While area not found

                //  While search limit not reached

                    //  Scan for other colliders that are not ground

                    //  If there are other colliders

                        //  Area not found start again


                    //  If area is within restricted spawn area
                    
                        //  Area not found start again

                    //  If path cannot be found from area to player or building

                        //  Area not found start again

                    //  All checks passed add location to spawn list

    }

    void CleanupAi()
    {

        //  For each enemy within the game

            //  Set should delete flag to true

            //  For each player within the game

                //  Calculate distance to player

                //  If distance is less than max distance
                    
                    //  Set should delete flag to false

            //  If should delete flag is true
                
                //  Delete enemy
    
    }


}
