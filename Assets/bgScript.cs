using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // Required for JToken and related types
using TMPro;

public class TextManager : MonoBehaviour
{
    public static string answerKey;

    public GameObject errorDetectorPrefab;

    public static TextManager Instance;



    private void Start()
    {
        GameObject debuggingSphere = GameObject.FindGameObjectWithTag("DebuggingSphere");
        GameObject debuggingCube = GameObject.FindGameObjectWithTag("DebuggingCube");
        debuggingSphere.GetComponent<Renderer>().material.color = Color.black;
        debuggingCube.GetComponent<Renderer>().material.color = Color.black;


        // I have tried for the past 6 hours to get either StreamingAssetsPath or PersistentDataPath to work but it just wont. This is what I have resorted to and I am terribly sorry.
        string stringifiedJson = @"{
    'ethane':
    [
            ['012012001'],
            ['012001', '012001']
    ],
    'propane':
    [
            ['012012001', '012012001'],
            ['012001', '012001', '012002']
    ],
    'butane':
    [
            ['012012001', '012012001', '012012001'],
            ['012001', '012001', '012002', '012002']
    ],
    'pentane':
    [
            ['012012001', '012012001', '012012001', '012012001'],
            ['012001', '012001', '012002', '012002', '012002']
    ],
    'hexane':
    [
            ['012012001', '012012001', '012012001', '012012001', '012012001'],
            ['012001', '012001', '012002', '012002', '012002', '012002']
    ],
    'heptane':
    [
            ['012012001', '012012001', '012012001', '012012001', '012012001', '012012001'],
            ['012001', '012001', '012002', '012002', '012002', '012002', '012002']
    ],
    'octane':
    [
            ['012012001', '012012001', '012012001', '012012001', '012012001', '012012001', '012012001'],
            ['012001', '012001', '012002', '012002', '012002', '012002', '012002', '012002']
    ],
    'nonane':
    [
            ['012012001', '012012001', '012012001', '012012001', '012012001', '012012001', '012012001', '012012001'],
            ['012001', '012001', '012002', '012002', '012002', '012002', '012002', '012002', '012002']
    ],
    'formate':
    [
            ['012016001', '012016002'],
            ['012002', '016001', '016001']
    ],
    'acetate':
    [
            ['012016001', '012016002', '012012001'],
            ['012003', '012001', '016001', '016001']
    ],
    'propionate':
    [
            ['012016001', '012016002', '012012001', '012012001'],
            ['012003', '012002', '012001', '016001', '016001']
    ],
    'butyrate':
    [
            ['012016001', '012016002', '012012001', '012012001', '012012001'],
            ['012003', '012002', '012002', '012001', '016001', '016001']
    ],
    'formaldehyde':
    [
            ['012016002'],
            ['012001', '016001']
    ],
    'nitrous oxide':
    [
            ['014014002', '014016002'],
            ['014001', '014002', '016001']
    ],
    'ozone':
    [
            ['016016001', '016016002'],
            ['016001', '016001', '016002']
    ],
    'arginine':
    [
            ['012016002', '012016001', '012012001', '012014001', '012012001','012012001','012012001', '012014001', '012014001', '012014001', '012014002'],
            ['016001', '016001', '012003', '012003', '014001', '012002', '012002', '012002', '012003', '014002', '014001', '014001']
    ],
    'lysine':
    [
            ['012016002', '012016001', '012012001', '012014001', '012012001', '012012001', '012012001', '012012001', '012014001'],
            ['016001', '016001', '012003', '012003', '014001', '012002', '012002', '012002', '012002', '014001']
    ],
    'glutamate':
    [
            ['012016002', '012016001', '012012001', '012014001', '012012001', '012012001', '012012001', '012016001', '012016002'],
            ['016001', '016001', '012003', '012003', '014001', '012002', '012002', '012003', '016001', '016001']
    ],
    'aspartate':
    [
            ['012016002', '012016001', '012012001', '012014001', '012012001', '012012001', '012016001', '012016002'],
            ['016001', '016001', '012003', '012003', '014001', '012002', '012003', '016001', '016001']
    ],
    'asparagine':
    [
            ['012016002', '012016001', '012012001', '012014001', '012012001', '012012001', '012014001', '012016002'],
            ['016001', '016001', '012003', '012003', '014001', '012002', '012003', '016001', '014001']
    ],
    'glutamine':
    [
            ['012016002', '012016001', '012012001', '012014001', '012012001', '012012001', '012012001', '012014001', '012016002'],
            ['016001', '016001', '012003', '012003', '014001', '012002', '012002', '012003', '016001', '014001']
    ],
    'serine':
    [
            ['012016002', '012016001', '012012001', '012014001', '012012001', '012016001'],
            ['016001', '016001', '012003', '012003', '014001', '012002', '016001']
    ],
    'threonine':
    [
            ['012016002', '012016001', '012012001', '012014001', '012012001', '012012001', '012016001'],
            ['016001', '016001', '012003', '012003', '014001', '012003', '012001', '016001']
    ],
    'alanine':
    [
            ['012016002', '012016001', '012012001', '012014001', '012012001'],
            ['016001', '016001', '012003', '012003', '014001', '012001']
    ],
    'valine':
    [
            ['012016002', '012016001', '012012001', '012014001', '012012001', '012012001', '012012001'],
            ['016001', '016001', '012003', '012003', '014001', '012003', '012001', '012001']
    ],
    'leucine':
    [
            ['012016002', '012016001', '012012001', '012014001', '012012001', '012012001', '012012001', '012012001'],
            ['016001', '016001', '012003', '012003', '014001', '012003', '012002', '012001', '012001']
    ],
    'isoleucine':
    [
            ['012016002', '012016001', '012012001', '012014001', '012012001', '012012001', '012012001', '012012001'],
            ['016001', '016001', '012003', '012003', '014001', '012003', '012002', '012001', '012001']
    ],
    'glycine':
    [
            ['012016002', '012016001', '012012001', '012014001'],
            ['016001', '016001', '012003', '012003', '014001']
    ]
}";
        debuggingSphere.GetComponent<Renderer>().material.color = Color.magenta;

        answerKey = NormalizeJson(stringifiedJson);
        debuggingSphere.GetComponent<Renderer>().material.color = Color.cyan;


        Vector3 position = new Vector3(0, 0, 0);
        debuggingSphere.GetComponent<Renderer>().material.color = Color.gray;

        GameObject errorDetectorObject = Instantiate(errorDetectorPrefab, position, Quaternion.identity);
        debuggingSphere.GetComponent<Renderer>().material.color = Color.blue;

        // Access the specific script component
        ErrorDetector errorDetector = errorDetectorObject.GetComponent<ErrorDetector>();
        if (errorDetector != null)
        {
            ErrorDetector.answerKey = JObject.Parse(answerKey);
            if (answerKey == null || ErrorDetector.answerKey.Count == 0)
            {
                debuggingSphere.GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                // debuggingSphere.GetComponent<Renderer>().material.color = Color.green;
            }
        } else {
            debuggingSphere.GetComponent<Renderer>().material.color = Color.red;
        }
        // if answerKey is size 0, color the sphere yellow

        errorDetector.initText();
    }

    public JObject getJObjectAnswerKey(){
        return JObject.Parse(answerKey);
    }

    public static string NormalizeJson(string json)
    {
        // Find the necessary GameObjects
        GameObject debuggingCube = GameObject.FindGameObjectWithTag("DebuggingCube");
        GameObject jsonSign = GameObject.FindGameObjectWithTag("json");

        // Add and configure TextMeshProUGUI component
        TextMeshProUGUI jsonTextMeshComponent = jsonSign.AddComponent<TextMeshProUGUI>();
        jsonTextMeshComponent.fontSize = 107;
        jsonTextMeshComponent.color = Color.white;

        JObject jObject = null;

        // 1. Parse the JSON into a JObject
        try
        {
            jObject = JObject.Parse(json);
        }
        catch (Exception ex1)
        {
            debuggingCube.GetComponent<Renderer>().material.color = Color.red; // Indicates JSON parsing error
            return null; // Exit early if JSON is invalid
        }

        // // 3. Update the TextMeshProUGUI text with the JObject count
        // try {
        //     if (jsonTextMeshComponent != null && jObject != null) {
        //         jsonTextMeshComponent.text = jObject.Count.ToString();
        //     }
        // }
        // catch {
        //     debuggingCube.GetComponent<Renderer>().material.color = Color.yellow; // Indicates error in setting text
        // }

        // 4. Sort the JSON
        try {
            SortJson(jObject);
            jsonTextMeshComponent.text = jObject.Count.ToString(); //WORKS AS EXPECTED PRINTS 28
        }
        catch (Exception ex4) {
            debuggingCube.GetComponent<Renderer>().material.color = Color.magenta; // Indicates error in sorting
        }

        try
        {
            jsonTextMeshComponent.text = jObject.ToString();
            return jObject.ToString(); //FAILS
        }
        catch (Exception ex5) {
            debuggingCube.GetComponent<Renderer>().material.color = Color.cyan; // Indicates error in final serialization
            //Save error message to string
            string errorMessage = $"Error serializing JSON: {ex5.Message}";
            jsonTextMeshComponent.text = $"JObject content: {jObject}";
            return null;
        }
    }

    private static void SortJson(JToken token)
    {
        if (token is JObject jObject)
        {
                       
            // Sort properties of JObject
            var properties = jObject.Properties().ToList();
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
            jArray.Clear();
            foreach (var value in values)
            {
                jArray.Add(value);
            }
        }
    }

}
