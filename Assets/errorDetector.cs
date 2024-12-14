using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;



public class ErrorDetector : MonoBehaviour
{

    public static GameObject text;
    public static JObject answerKey;

    private static JToken currJToken;

    private static string currName;


    public void initText(){
        GameObject debuggingCube = GameObject.FindGameObjectWithTag("DebuggingCube");
        // debuggingCube.GetComponent<Renderer>().material.color = Color.green;
        getRandomName();
        UpdatePanelText(currName);
    }

    private static int counter = -1;

   public void UpdatePanelText(string nextName)
{
    // Find the panel by tag
    GameObject panel = GameObject.FindGameObjectWithTag("Mission");
    if (panel == null)
    {
        Debug.LogError("Panel with tag 'Mission' not found!");
        return;
    }

    // Get the TMP_Text component from the panel
    TextMeshProUGUI missionText = panel.GetComponent<TextMeshProUGUI>();
    if (missionText == null)
    {
        Debug.LogWarning("TMP_Text component not found on the panel! Adding one...");
        missionText = panel.AddComponent<TextMeshProUGUI>(); // Add TMP_Text if not found
    }

    // Update the text on the panel
    string newText = "create " + nextName;
    missionText.text = newText;
    missionText.fontSize = 20;

    Debug.Log($"Updated panel text to: {newText}");

    // Find the score by tag
    GameObject score = GameObject.FindGameObjectWithTag("Score");

    // Get the TMP_Text component from the score
    TextMeshProUGUI scoreText = score.GetComponent<TextMeshProUGUI>();
    if (scoreText == null)
    {
        Debug.LogWarning("TMP_Text component not found on the score! Adding one...");
        scoreText = score.AddComponent<TextMeshProUGUI>(); // Add TMP_Text if not found
    }

    // Update the text on the score
    counter++;
    newText = "Score: " + counter;
    scoreText.text = newText;
    scoreText.fontSize = 150;
    scoreText.color = Color.green;
}



    private void getRandomName(){
        GameObject debuggingCube = GameObject.FindGameObjectWithTag("DebuggingCube");
        // debuggingCube.GetComponent<Renderer>().material.color = Color.blue;
        // Step 1: Get all keys as a list
        List<string> keys = new List<string>(answerKey.Properties().Select(p => p.Name));

        // Step 2: Create a Random object and generate a random index
        System.Random random = new System.Random();
        int randomIndex = random.Next(keys.Count);

        // Step 3: Get the random key and its value
        currName = keys[randomIndex];
        currJToken = answerKey[currName];
        // debuggingCube.GetComponent<Renderer>().material.color = Color.red;

        // Output the result
        Debug.Log($"Random Key: {currName}, Random Value: {currJToken}");
    }

    public void CheckIfCurrModelMatchesUserModel()
    {
        // Step 1: Generate list of all bonds
        GameObject[] bonds = GameObject.FindGameObjectsWithTag("Bond");
        List<string> notationBonds = new List<string>();
        foreach (GameObject bond in bonds)
        {
            // Get the parent of the bond
            Transform parentTransform = bond.transform.parent;
            string atom1 = parentTransform.tag;
            
            // Get the BondUpdater component
            BondUpdater bondUpdater = bond.GetComponent<BondUpdater>();
            string atom2 = bondUpdater.equippedAtom.tag;

            string bondOrder = bondUpdater.getBondOrder();
            // Get the masses of both atoms
            string mass1 = GetMass(atom1);
            string mass2 = GetMass(atom2);

            // Parse masses as integers to compare
            int mass1Int = int.Parse(mass1);
            int mass2Int = int.Parse(mass2);

            // Determine smallest and largest mass
            string smallestMass = mass1Int < mass2Int ? mass1 : mass2;
            string largestMass = mass1Int > mass2Int ? mass1 : mass2;

            // Concatenate the result in "SmallestMass, LargestMass, BondOrder" format
            string bondNotation = $"{smallestMass}{largestMass}{bondOrder}";

            // Add to the notation list
            notationBonds.Add(bondNotation);
        }

        // Step 2: Generate list of all atoms
        GameObject[] atoms = GameObject.FindGameObjectsWithTag("O")
            .Concat(GameObject.FindGameObjectsWithTag("N"))
            .Concat(GameObject.FindGameObjectsWithTag("C"))
            .ToArray();

        List<string> notationAtoms = new List<string>();
        foreach (GameObject atom in atoms)
        {
            
            int coordinationNumber = 0;
            
            // Check if the atom is inside a socket
            foreach (GameObject bond in bonds)
            {
                BondUpdater bondUpdater = bond.GetComponent<BondUpdater>();
                GameObject tempAtom = bondUpdater.equippedAtom;
                if (tempAtom == atom){coordinationNumber++;}
            }
            

            // Count how many children with the "Bond" tag the atom has
            foreach (Transform child in atom.transform)
            {
                if (child.CompareTag("Bond"))
                {
                    coordinationNumber++;
                }
            }

            string currAtom = "";
            switch (coordinationNumber){
                case 1:
                    currAtom = "001";
                    break;
                case 2:
                    currAtom = "002";
                    break;
                case 3:
                    currAtom = "003";
                    break;
                default:
                    currAtom = "NA";
                    break;
            }
            if (currAtom != "NA"){notationAtoms.Add(GetMass(atom.tag) + currAtom);}
        }

        JToken tempJson = turnIntoJToken(notationBonds, notationAtoms);
        SortJson(tempJson);
        Debug.Log(tempJson+"is the temp Json");
        string tempString = "";
        if (tempJson != null){
            tempString = tempJson.ToString(Formatting.None);
        }
        string currString = currJToken.ToString(Formatting.None);

        
        if (currString.Equals(tempString)){
            Debug.Log("Structure correct");
            initText();
        } else {
            Debug.Log("keep building..."+tempJson);
        }
    }

    private JToken turnIntoJToken(List<string> notationBonds, List<string> notationAtoms)
{
    // Create a JArray to hold the main structure
    JArray jsonStructure = new JArray
    {
        // Add a JArray containing the bonds as a single string
        new JArray( notationBonds),

        // Add a JArray containing the atoms as separate strings
        new JArray(notationAtoms)
    };

    // Return the structure as a JToken
    return jsonStructure;
}




    private string GetMass(string name)
    {
        switch (name)
        {
            case "C":
                return "012";
            case "N":
                return "014";
            case "O":
                return "016";
            default:
                return "Unknown"; // Optional: handle unexpected input
        }
    }

    public static string NormalizeJson(string json)
    {
        // Parse the JSON into a JObject
        JObject jObject = JObject.Parse(json);

        // Recursively sort the JSON
        SortJson(jObject);

        // Convert back to a string
        return jObject.ToString();
        // return JsonConvert.SerializeObject(jObject, Formatting.Indented);

    }

    private static void SortJson(JToken token){
        if (token is JObject jObject)
        {
            // Sort properties of JObject
            var properties = jObject.Properties();
            foreach (var property in properties)
            {
                SortJson(property.Value);
            }
        }
        else if (token is JArray jArray)
        {
            // Sort elements in JArray
            var values = jArray.ToList();
            foreach (var value in values)
            {
                SortJson(value);
            }

            values.Sort((x, y) => string.Compare(x.ToString(), y.ToString(), StringComparison.Ordinal));
            jArray.ReplaceAll(values);
        }
    }

}