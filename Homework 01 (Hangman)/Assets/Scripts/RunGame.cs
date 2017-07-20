/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-09-01
 * Assignment:  Homework 1
 * Source File: RunGame.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Controls the hangman game for homework 1.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;
using System.Collections;

public class RunGame : MonoBehaviour {
    // Public class variables.
    public GameObject historyRef;
    public GameObject[] parts;

    // Private class variables.
    private string[] words = new string[] {
        "random",
        "words",
        "go",
        "here",
        "along",
        "with",
        "some",
        "other",
        "cool",
        "stuff"
    };
    private string word = "";
    private string guess = "";
    private string history = "";
    private int leftToGuess;
    private bool[] guessVisible;
    private int missed;

    /*----------------------------------------------------------------------------------------------
     * Name:    ChooseWord
     * Type:    Method
     * Purpose: Chooses a random word and sets up the game space accordingly.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void ChooseWord() {
        // Choose a random word and set up the support variables.
        int i = Random.Range(0, words.Length);
        word = words[i];
        guessVisible = new bool[word.Length];
        guess = "";
        missed = 0;

        // Set up the blank word space for the chosen word and updates the appropriate game object.
        for (i = 0; i < word.Length; i++) {
            guess += "_ ";
            guessVisible[i] = false;
        }
        GetComponent<TextMesh>().text = guess;
        leftToGuess = guessVisible.Length;

        // Output the chosen word out into the debug log for testing purposes.
        //Debug.Log("Chosen word: " + word);
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    CheckAddHistory
     * Type:    Method
     * Purpose: Validates that a character is a letter and has not already been added, then adds it
     * 			to the history.
     * Input:   char letter, containing the character to validate.
     * Output:  bool, representing whether the character was valid and was not played yet (true), or
     * 			whether the character was invalid or already played (false).
    ----------------------------------------------------------------------------------------------*/
    bool CheckAddHistory(char letter) {
        // Check to see if the character has already been played.
        if (history.Contains("" + letter)) {
            return false;
        }
        // Check to see if the character actually is a letter.
        else if (!char.IsLetter(letter)) {
            return false;
        }
        // Else the character is a letter and has not been played.
        else {
            // Add letter to history with a spacer and update the history game object.
            history += letter + " ";
            historyRef.GetComponent<TextMesh>().text = history;
            return true;
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    CheckAddGuess
     * Type:    Method
     * Purpose: Checks to see whether a character is a match for any letter in the game's chosen
     *          word.
     * Input:   char letter, contains the character to be checked.
     * Output:  bool, representing whether the guess was a hit (true) or a miss (false).
    ----------------------------------------------------------------------------------------------*/
    bool CheckAddGuess(char letter) {
        bool foundMatch = false;

        // Iterate through the word.
        for (int position = 0; position < word.Length; position++) {
            // Case insensitive check to see if a letter at a position in the word is a match for
            // the method argument.
            if (char.ToUpper(word[position]) == letter) {
                // If a match is found, mark that letter as found, decrement the number of letters
                // left to guess, and mark this method as having found a letter.
                guessVisible[position] = true;
                leftToGuess--;
                foundMatch = true;
            }
        }

        return foundMatch;
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    PlayLetter
     * Type:    Method
     * Purpose: The main controlling method of how letters are handled as they are played.
     * Input:   char letter, contains the character pressed on the keyboard.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void PlayLetter(char letter) {
        // Convert the input letter to uppercase.
        letter = char.ToUpper(letter);

        // Check if the game has ended. If it has, ignore input and return.
        if (missed > 7 || leftToGuess == 0) {
            return;
        }
        // Check if the letter is valid and hasn't already been played.
        else if (CheckAddHistory(letter)) {
            // Check to see if the guessed letter is a hit.
            if (CheckAddGuess(letter)) {
                // Check to see if the player has won the game.
                if (leftToGuess == 0) {
                    // If player has won, display message to that effect.
                    historyRef.GetComponent<TextMesh>().text = "You Won!";
                }

                // Reset guess and then iterate through the guessVisible array, displaying letters
                // where needed, otherwise displaying underscores.
                guess = "";
                for (int position = 0; position < guessVisible.Length; position++) {
                    if (guessVisible[position]) {
                        guess += word[position] + " ";
                    }
                    else {
                        guess += "_ ";
                    }
                }
                // Update the game object.
                GetComponent<TextMesh>().text = guess;
            }
            // Otherwise guessed letter is a miss.
            else {
                // Increment missed counter.
                missed++;

                // Check to see if there are any body parts left to display.
                if (missed <= parts.Length) {
                    // Make a body part appear.
                    parts[missed - 1].SetActive(true);
                }
                // Otherwise the game has been lost.
                else {
                    // Update the display to let the player know they have lost.
                    historyRef.GetComponent<TextMesh>().text = "You Lost!";

                    // Reset guess and then iterate through the word, displaying all letters.
                    guess = "";
                    foreach (char wordLetter in word)
                        guess += wordLetter + " ";

                    // Update the game object.
                    GetComponent<TextMesh>().text = guess;
                }
            }
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    OnStart
     * Type:    Method
     * Purpose: Used for initialization.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void Start() {
        // Initialize the history text.
        historyRef.GetComponent<TextMesh>().text = "";

        // Choose word for the game instance.
        ChooseWord();
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    OnGUI
     * Type:    Method
     * Purpose: Callback function for keyboard and mouse actions through events.
     * Input:   
     * Output:  
    ----------------------------------------------------------------------------------------------*/
    void OnGUI() {
        // Get the current event.
        Event e = Event.current;

        // Check to see if it's a keyboard event.
        if (e.isKey) {
            // If event is a keyboard event, attempt to play the letter.
            PlayLetter(e.character);
        }
    }
}
