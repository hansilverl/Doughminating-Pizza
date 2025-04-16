using System.Collections;
using System.Collections.Generic;


public interface IInteractable 
{
    void Interact(); // Method that will be called when the player interacts with the object
    string getInterationText(); // Text that will be displayed when the player is close to the object
}