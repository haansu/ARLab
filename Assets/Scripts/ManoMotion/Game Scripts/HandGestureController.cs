using UnityEngine;

namespace AR_ManoMotion
{
    public class HandGestureController : MonoBehaviour
    {
        private FruitSpawner _fruitSpawner;

        void Start() => _fruitSpawner = GetComponentInChildren<FruitSpawner>();

        void Update()
        {
            /* TODO 3.1, 4.1, 5.1 Get the hand info */
            
            /* TODO 3.2, 4.2, 5.2 Get the gesture info */

            /* TODO 3.3. Get the hand side information */

            /* TODO 4.3. Get the continuous gestures */
            
            /* TODO 5.3. Get the trigger gestures */
            
            /* TODO 3.5. When the hand side is 'HandSide.Palmside', disable the fruit spawning altogether.
             * If it's 'HandSide.Backside' spawning should be active (default behavior)
             */
            
            /* TODO 4.5. For each frame in which the continuous gesture 'CLOSED_HAND_GESTURE' is active, 
             * increase the spawn rate by 20 (or some number to see a difference)
             * For any other continuous gesture the spawn rate should be kept as default
             * Hint! Use the spawn rate variable found in the fruit spawner script
             * You need to figure out where to use it, changing it's value won't do anything as it's not used in code
             */
            
            /* TODO 5.4. When the trigger gesture 'PICK' is detected, destroy all fruit instances which are on-screen.
             * Hint! Use 'DestroyFruitInstance()' function found on fruit controller script,
             * as it plays particle and sound effect as well
             */
            
        }
    }
}