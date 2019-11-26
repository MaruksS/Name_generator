using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;


namespace CIS_Alias_generator
{
    public partial class Form1 : Form
    {
        //create dictionary array of transliteration symbol objects
        transliteration[] dictionary = new transliteration[100];
        ExceptionRules[] rules = new ExceptionRules[20];
        word[] userInput = new word[20];
        string[] indicators = new string[50];
        string[,] allCompanyAbbreviations = new string[20, 20];

        string[] companyAliases = new string[20];

        string configFile = Properties.Settings.Default.Config;

        int[] exceptionsInWord;

        //resets configuration
        private void reset()
        {
            userInput = new word[20];
            dictionary = new transliteration[100];
            rules = new ExceptionRules[20];
            indicators = new string[50];
            allCompanyAbbreviations = new string[20, 20];
            companyAliases = new string[20];
        }


        private void readConfig()
        {
            string line;
            string mode = "";
            int count = 0;

            // Read the file and save rules
            StreamReader file =
                new StreamReader(configFile);
            while ((line = file.ReadLine()) != null)
            {
                if (line == "[alphabet]")
                {
                    mode = "alphabet";
                    count = 0;
                }
                if (line == "[rules]")
                {
                    mode = "rules";
                    count = 0;
                }
                if (line == "[company]")
                {
                    mode = "company";
                    count = 0;
                }
                if (line == "[type1]")
                {
                    mode = "type1";
                    count = 0;
                }

                //add each letter with transliteration to the dictionary
                if (mode == "alphabet")
                {
                    string[] letters = line.Split('-');
                    if (letters.Length > 1)
                    {
                        dictionary[count] = new transliteration(letters[0], letters[1]);
                    }
                    else
                    {
                        dictionary[count] = new transliteration(letters[0], "");
                    }
                    count++;
                }

                //TODO: Rule handling
                if (mode == "rules")
                {
                    string[] transliterationParameters = line.Split(':');
                    if (transliterationParameters.Length >= 4)
                    {
                        string[] possibilities = new string[transliterationParameters.Length-3];
                        for (int i = 3; i < transliterationParameters.Length; i++)
                        {
                            possibilities[i - 3] = transliterationParameters[i];
                        }
                        rules[count] = new ExceptionRules(transliterationParameters[0], transliterationParameters[1], transliterationParameters[2], possibilities);
                        count++;
                    }
                }
                if (mode == "type1")
                {
                    string symbol = line;
                    indicators[count] = symbol;
                    count++;
                }

                //add each company abbreviation to the abbreviation list
                if (mode == "company")
                {
                    string[] companyArray = line.Split('~');
                    for (int i = 0; i < companyArray.Length; i++)
                    {
                        allCompanyAbbreviations[count, i] = companyArray[i];
                    }
                    count++;
                }
            }
            file.Close();
        }

        public Form1()
        {
            InitializeComponent();

            //if configfile is present
            if (configFile != "" && File.Exists(configFile))
            {
                readConfig();
            }
        }


        private string transliterate(string input, int type)
        {
            // final output string
            string resultTotal = "";
            string englishResult = "";

            companyAliases = new string[20];

            //temporary value to store translation 
            string nativeResult="";

            int row = 0;

            //abbreviation indicators
            bool abbreviationBehind = false;
            bool abbreviationFront = false;
            bool noAbbreviation = false;

            string[] separated = null;

            //split input into string values array
            separated = input.Split(null);

            exceptionsInWord = new int[separated.Length];

            string[] variations = new string[0];
            int maxException = 0;
            for (int i = 0; i < separated.Length; i++)
            {
                int exceptionCount = countExceptions(separated[i]);
                if (maxException < exceptionCount) maxException = exceptionCount;
            }

            /* 
            * if type person
            */
            if (type == 1)
            {
                for (int i = 0; i < separated.Length; i++)
                {
                    if (maxException == 0) variations = simpleTranslit(separated[i], maxException);
                    else variations = AltTransliterateWord(separated[i], maxException, maxException);

                    userInput[i] = new word(separated[i], variations, maxException, i);
                }
                int variationCount = countVariations();

                string[] aliases = new string[variationCount];

                //create aliases for max variations
                for (int i = 0; i < variationCount; i++)
                {
                    if (i==0) aliases[i] = "";

                    for (int j = 0; j < userInput.Length; j++)
                    {
                        if (userInput[j]!=null)
                        {
                            //if no variation available, use the standard one
                            if (userInput[j].getVariation()[i] == null) aliases[i] = aliases[i] + userInput[j].getVariation()[0] + " ";
                            else aliases[i] = aliases[i] + userInput[j].getVariation()[i] + " ";

                            //if first word, make capitals
                            if (j == 0) aliases[i] = aliases[i].Trim().ToUpper() + ",";
                        }
                    }
                    
                }

                //create native alias
                for (int i = 0; i < userInput.Length; i++)
                {
                    if (userInput[i]!=null)
                    {
                        nativeResult = nativeResult + FirstCharToUpper(userInput[i].getNative().ToLower()) + " ";
                        if (i == 0) nativeResult = nativeResult.Trim().ToUpper() + ",";
                    }
                }

                //unite aliases
                for (int i = 0; i < aliases.Length; i++)
                {
                    englishResult = englishResult + aliases[i] + Environment.NewLine;
                }
                //final output
                resultTotal = nativeResult + Environment.NewLine + englishResult;
            }

            /* 
            * if type entity
            */
            else
            {
                //Transliterate each string value of input individually
                for (int i = 0; i < separated.Length; i++)
                {
                    // if 1st element of input
                    if (i == 0)
                    {
                        //Find first and last word of input
                        string lastWord = separated[separated.Length-1];
                        string firstWord = separated[0];
                        row = 0;

                        //check if last/first word of input is inside abbreviation list
                        for (int j = 0; j < allCompanyAbbreviations.GetLength(0); j++)
                        {
                            //if abbreaviation is first word
                            if (allCompanyAbbreviations[j, 0] == firstWord | allCompanyAbbreviations[j, 1] == firstWord)
                            {
                                abbreviationFront = true;
                                row = j;
                                break;
                            }
                            //if abbreaviation is last word
                            else if (allCompanyAbbreviations[j, 0] == lastWord | allCompanyAbbreviations[j, 1] == lastWord)
                            {
                                abbreviationBehind = true;
                                row = j;
                                break;
                            }
                        }

                        // add abbreviation if found
                        if (abbreviationBehind | abbreviationFront)
                        {
                            // add all abbreviation translations to alias
                            for (int j = 1; j < allCompanyAbbreviations.GetLength(0); j++)
                            {
                                int x = j - 1;
                                if (allCompanyAbbreviations[row, j] != null)
                                {
                                    companyAliases[x] = allCompanyAbbreviations[row, j] + " ";
                                }
                            }

                            // if abbreviation was behind name, put it upfront and add 1st word
                            if (abbreviationBehind)
                            {
                                addToCompanyName(separated[i], noAbbreviation, row, i);
                            }
                        }

                        //if no abbreviation found, translate
                        else
                        {
                            companyAliases[0] = "";
                            companyAliases[1] = "";
                            noAbbreviation = true;
                            addToCompanyName(separated[i], noAbbreviation, row, i);  
                        }
                    } //end of (if 1st element of input)

                    //if 2nd> element, transliterate and add to alias
                    else
                    {
                        if (abbreviationBehind && i == separated.Length-1) break;

                        else
                        {
                            addToCompanyName(separated[i], noAbbreviation, row, i);
                        }
                    }
                } //End of for: Transliterate each string value of input individually

                //after translating all aliases add them to result string
                for (int i = 0; i < companyAliases.Length; i++)
                {
                    if (companyAliases[i] !=null)
                    {
                        resultTotal = resultTotal + companyAliases[i] + Environment.NewLine;
                    }
                }

                if (maxException>0)
                {
                    int separatedLength = 0;
                    if (abbreviationBehind)
                    {
                        separatedLength = separated.Length - 1;
                    }
                    else if (abbreviationFront)
                    {
                        separated = separated.Skip(1).ToArray();
                        separatedLength = separated.Length;
                    }
                    else separatedLength = separated.Length - 1;
                    for (int i = 0; i < separatedLength; i++)
                    {
                        variations = AltTransliterateWord(separated[i], maxException, maxException);

                        userInput[i] = new word(separated[i], variations, maxException, i);
                    }
                    int variationCount = countVariations();

                    string[] aliases = new string[variationCount];

                    //create aliases for max variations
                    for (int i = 0; i < variationCount; i++)
                    {
                        if (i == 0) aliases[i] = "";

                        for (int j = 0; j < userInput.Length; j++)
                        {
                            if (userInput[j] != null)
                            {
                                //if no variation available, use the standard one
                                if (userInput[j].getVariation()[i] == null) aliases[i] = aliases[i] + userInput[j].getVariation()[0] + " ";
                                else aliases[i] = aliases[i] + userInput[j].getVariation()[i] + " ";
                                aliases[i] = aliases[i].Trim().ToUpper() + " ";
                            }
                        }

                    }
                    //unite aliases
                    for (int i = 0; i < aliases.Length; i++)
                    {
                        resultTotal = resultTotal + allCompanyAbbreviations[row, 5] + " " + aliases[i] + Environment.NewLine;
                    }
                }
            } //End of (if entity)
            return resultTotal;
        }

        //get the biggest variations count
        private int countVariations()
        {
            int maxVariations = 0;

            for (int i = 0; i < userInput.Length; i++)
            {
                if (userInput[i] != null)
                {
                    string[] allVar = userInput[i].getVariation();
                    int variations = 0;
                    for (int j = 0; j < allVar.Length; j++)
                    {
                        if (allVar[j] != null) variations++;
                    }
                    if (variations > maxVariations) maxVariations = variations;
                }
            }
            return maxVariations;
        }

       /* 
        * translate single word
        */
        private string[] AltTransliterateWord(string input, int exceptionCount, int maxException)
        {
            input = input.ToLower();
            string word;
            /* 
             * separates string input into character array
             */
            char[] separated = input.ToCharArray();

            // transliterated final word
            string[] result = new string[50];

            // temporary variable to hold currently viewed character
            char current;

            // temporary variable to hold special symbol, to which special rules will apply (from object)
            char specialSymbol;

            // temporary variable to hold special symbol replacement (from object)
            string[] replacement;

            //if special symbol (for the rule handling)
            bool special = false;

            // temporary variable to hold english transliteration
            string english = "";

            int replacementCount = 1;
            int variation = 0;
            int exceptionNr = 0;
            int specialNr = 0;
            int count = 0;

            for (int x = 0; x < maxException; x++)
            {
                exceptionNr = x;
                for (int i = 0; i < exceptionCount; i++)
                {
                    for (variation = 0; variation < replacementCount; variation++)
                    {
                        word = "";
                        specialNr = 0;
                        //loop through each character of a word
                        for (int y = 0; y < separated.Length; y++)
                        {
                            //select current character
                            current = separated[y];

                            //TODO: apply dynamic rule handling
                            if (rules[0] != null)
                            {
                                bool exceptionFound = false;
                                for (int j = 0; j < rules.Length; j++)
                                {
                                    if (rules[j] != null)
                                    {
                                        specialSymbol = rules[j].getSymbol().ToCharArray()[0];

                                        //apply rule handling on character
                                        if (special & current == specialSymbol || (y == 0 && indicators.Contains(" ") & current == specialSymbol))
                                        {
                                            if (specialNr==exceptionNr || count ==0 )
                                            {
                                                replacement = rules[j].getReplacement();
                                                replacementCount = replacement.Length;

                                                //TODO: multiple variations for exceptions
                                                english = replacement[variation];
                                                if (y == 0)
                                                {
                                                    english = FirstCharToUpper(english);
                                                }
                                                exceptionFound = true;

                                                specialNr++;
                                                break;
                                            }
                                            specialNr++;
                                        }
                                    }
                                }

                                if (!exceptionFound)
                                {
                                    english = findCharTransliteration(current, y);
                                }
                            }

                            // if special rules doesn't apply for character
                            else
                            {
                                english = findCharTransliteration(current, y);
                            }

                            //check if current character is an indicator of special rule
                            special = checkSpecial(current);

                            //add transliteration to result
                            word = word + english;
                        }//end of loop through each character of a word
                        if (!result.Contains(word))
                        {
                            result[count] = word;
                            count++;
                        }
                    }//end of variation loop
                } //end max exception loop
            }//end max exception count loop

            //return words
            return result;
        }

        private string[] simpleTranslit(string input, int length)
        {
            if (length==0)
            {
                length = 1;
            }
            // transliterated final word
            string[] result = new string[length];

            /* 
            * separates string input into character array
            */
            char[] separated = input.ToCharArray();

            // temporary variable to hold currently viewed character
            char current;

            // temporary variable to hold english transliteration
            string english = "";


            //loop through each character of a word
            for (int i = 0; i < separated.Length; i++)
            {
                //select current character
                current = separated[i];

                english = findCharTransliteration(current, i);

                //add transliteration to result
                result[0] = result[0] + english;
            }

            return result;
        }

        //translate company name
        private void addToCompanyName(string text, bool noAbbreviation, int row, int wordNr)
        {
            //check if alias contains native symbols
            bool isNative = false;

            //how many alias outputs are neccesary
            int times = 2;

            //if no abbreviation found add 2 aliases (native and transliterated)
            if(noAbbreviation)
            {
                companyAliases[0] = companyAliases[0] + text.ToUpper() + " ";
                companyAliases[1] = companyAliases[1] + transliterateWord(text, true, wordNr, 0) + " ";
            }

            //if abbreviation found
            else
            {
                //nr of loops is equal to nr of abbreviation translations
                times = companyAliases.Length;

                //loop through each alias
                for (int j = 0; j < times; j++)
                {
                    //set default as non-native
                    isNative = false;

                    if (companyAliases[j] != null)
                    {
                        //convert word to char array and check if it contains native symbols
                        char[] aliasCharacterArray = companyAliases[j].ToCharArray();
                        for (int i = 0; i < aliasCharacterArray.Length; i++)
                        {
                            if (checkDictionary(aliasCharacterArray[i]))
                            {
                                isNative = true;
                                break;
                            }
                        }
                        //if contains native symbols - do not transliterate
                        if (isNative)
                        {
                            companyAliases[j] = companyAliases[j] + text.ToUpper() + " ";
                        }
                        //if no native symbols found - transliterate
                        else
                        {
                            companyAliases[j] = companyAliases[j] + transliterateWord(text, true, wordNr, 0) + " ";
                        }
                    }
                }//end of loop through each alias
            }//end of if abbreviation found
        }

        /* 
        * translate single word
        */
        private string transliterateWord(string input, bool capitals, int wordNr, int exceptionNr)
        {
            // transliterated final word
            string result = "";

            /* 
             * separates string input into character array
             */
            char[] separated = input.ToCharArray();

            // temporary variable to hold currently viewed character
            char current;

            // temporary variable to hold special symbol, to which special rules will apply (from object)
            char specialSymbol;

            // temporary variable to hold special symbol replacement (from object)
            string[] replacement;

            //if special symbol (for the rule handling)
            bool special = false;

            // temporary variable to hold english transliteration
            string english = "";


            //loop through each character of a word
            for (int i = 0; i < separated.Length; i++)
            {
                //select current character
                current = separated[i];

                //TODO: apply dynamic rule handling
                if (rules[0] != null)
                {
                    bool exceptionFound = false;
                    for (int j = 0; j < rules.Length; j++)
                    {
                        if (rules[j] != null)
                        {
                            specialSymbol = rules[j].getSymbol().ToCharArray()[0];
                            
                            //apply rule handling on character
                            if (special & current == specialSymbol || (i == 0 && indicators.Contains(" ") & current == specialSymbol))
                            {
                                replacement = rules[j].getReplacement();
                               //TODO: multiple variations for exceptions
                                english = replacement[exceptionNr];
                                if (i == 0)
                                {
                                    english = FirstCharToUpper(english);
                                }
                                exceptionFound = true;
                                break;
                            }
                        }
                    }
                    if (!exceptionFound)
                    {
                        english = findCharTransliteration(current, i);
                    }
                }

                // if special rules doesn't apply for character
                else
                {
                    english = findCharTransliteration(current, i);
                }

                //check if current character is an indicator of special rule
                special = checkSpecial(current);

                //if capitals required, transform
                if (capitals && english != null)
                {
                    english = english.ToUpper();
                }
                //add transliteration to result
                result = result + english;
            }//end of loop through each character of a word

            //return word
            return result;
        }

        private int countExceptions(string input)
        {
            //count of exceptions in a word
            int exceptionCount = 0;

            /* 
             * separates string input into character array
             */
            char[] separated = input.ToCharArray();

            // temporary variable to hold currently viewed character
            char current;

            // temporary variable to hold special symbol, to which special rules will apply (from object)
            char specialSymbol;

            // temporary variable to hold special symbol replacement (from object)
            string[] replacement;

            //if special symbol (for the rule handling)
            bool special = false;

            //loop through each character of a word
            for (int i = 0; i < separated.Length; i++)
            {
                //select current character
                current = separated[i];

                //TODO: apply dynamic rule handling
                if (rules[0] != null)
                {
                    for (int j = 0; j < rules.Length; j++)
                    {
                        if (rules[j] != null)
                        {
                            specialSymbol = rules[j].getSymbol().ToCharArray()[0];

                            //apply rule handling on character
                            if (special & current == specialSymbol || (i == 0 && indicators.Contains(" ") & current == specialSymbol))
                            {
                                replacement = rules[j].getReplacement();

                                for (int x = 0; x < replacement.Length; x++)
                                {
                                    exceptionCount++;
                                }
                            }
                        }
                    }
                }
                //check if current character is an indicator of special rule
                special = checkSpecial(current);
            }
            return exceptionCount;
        }

        private string findCharTransliteration(char current, int i)
        {
            // temporary variable to hold english transliteration
            string english = "";

            //loop through dictionary to find current character
            for (int j = 1; j < dictionary.Length; j++)
            {
                char original = ' ';
                if (dictionary[j] != null)
                {
                    original = char.Parse(dictionary[j].getOriginal());

                    //if current character is found in dictionary, transliterate
                    if (current == original)
                    {
                        english = dictionary[j].getTranslit();
                        //if first character of the word, make it upper case
                        if (i == 0) english = FirstCharToUpper(english);
                        //break outside dictionary loop
                        break;
                    }
                    //if current character is not found in dictionary, keep it
                    else
                    {
                        english = current.ToString();
                    }
                }
            }

            return english;
        }

        /* 
         * Check if current character falls under special rule
         */
        private bool checkSpecial(char current)
        {
            char indicator;
            bool special = false;

            for (int x = 0; x < indicators.Length; x++)
            {
                if (indicators[x] != null)
                {
                    indicator = indicators[x].ToCharArray()[0];
                    if (indicator == current)
                    {
                        special = true;
                        break;
                    }
                    else special = false;
                }
            }

            return special;
        }

        /* 
         * Check if current character falls under special rule
         */
        private bool checkDictionary(char current)
        {
            char indicator;
            bool special = false;
            
            for (int x = 0; x < dictionary.Length; x++)
            {
                
                if (dictionary[x] != null)
                {
                    indicator = dictionary[x].getOriginal().ToCharArray()[0];
                    if (indicator == current)
                    {
                        special = true;
                        break;
                    }
                    else special = false;
                }
            }
            return special;
        }

        /* 
         * Trigger button activity
         */
        private void onBtnClick(object sender, EventArgs e)
        {
            txt_output.Text = "";
            userInput = new word[20];
            string input = txt_input.Text.Trim();
            string result = "";

            if (chb_Person.Checked == false)
            {
                result = transliterate(input, 1);
            }else
            {
                result = transliterate(input, 2);
            }

            txt_output.Text = result;

        }

        private void selectConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //configFile = Path.GetDirectoryName(openFileDialog1.FileName);
                configFile = openFileDialog1.FileName;
                Properties.Settings.Default.Config = configFile;
                Properties.Settings.Default.Save();
                reset();
                readConfig();
            }
        }

        /* 
         * Change first character of string to UPPER CASE
         */
        private string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

    }

}
