using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        private const string PATH_TO_INDEXES = "pack_0";
        private const string PATH_TO_WORDS = "words_list";

        private const string FOLDER_NAME = "Fillwords/";

        public GridFillWords LoadModel(int index)
        {
            GridFillWords grid = null;
            index--; //работа с массивом

            try
            { 
                var indexesText = Resources.Load<TextAsset>(FOLDER_NAME + PATH_TO_INDEXES)?.text;
                var wordsText = Resources.Load<TextAsset>(FOLDER_NAME + PATH_TO_WORDS)?.text;

                //удаление r
                indexesText = indexesText.Replace("\r", "");
                wordsText = wordsText.Replace("\r", "");

                string[] levels = indexesText.Split('\n');
                string[] words = wordsText.Split('\n');

                if(index > levels.Length - 1)
                    throw new Exception("No such index in files");

                for (int i = index; i < levels.Length; i++)    
                {
                    if (grid != null)
                        break;

                    var level = levels[i];

                    grid = TryLoadLevel(level, words);
                }

            }
            catch
            {
                throw new Exception("Error while parsing files");
            }

            if(grid == null)
                throw new Exception("No correct level in files");

            return grid;
        }

        private void ParseLevel(string level, ref List<int> words_indexes, ref List<List<int>> letter_indexes)
        {
            var parts = level.Split(" ");

            foreach (var part in parts)
            {
                if (part.Contains(';'))
                {
                    var indexes = part.Split(';');

                    letter_indexes.Add(new List<int>(indexes.Length));

                    foreach (var indexStr in indexes)
                    {
                        letter_indexes.Last().Add(Convert.ToInt32(indexStr));
                    }
                }
                else
                    words_indexes.Add(Convert.ToInt32(part));
            }
        }

        private bool CheckSquareGround(in int numOfLetters)
        {
            if (Mathf.Sqrt(numOfLetters) % 1f != 0f)
                return false;

            return true;
        }

        private  bool CheckMaxIndex(List<List<int>> letterIndexes, in int numOfLetters)
        {
            var maxIndex = letterIndexes[0].Max();
            for (int i = 1; i < letterIndexes.Count; i++)
            {
                var value = letterIndexes[i].Max();
                if (value > maxIndex)
                    maxIndex = value;
            }

            //макс индекс
            if (numOfLetters <= maxIndex)
                return false;

            return true;
        }

        private bool CheckUniqueLetters(List<List<int>> letterIndexes, in int numOfLetters)
        {
            HashSet<int> uniqueIndexes = new HashSet<int>(numOfLetters);
            for (int i = 0; i < letterIndexes.Count; i++)
            {
                for (int j = 0; j < letterIndexes[i].Count; j++)
                    if (!uniqueIndexes.Add(letterIndexes[i][j]))
                        return false;
            }

            return true;
        }

        private bool CheckLevel(List<int> wordsIndexes, List<List<int>> letterIndexes, 
            string[] words, out int numOfLetters)
        {
            numOfLetters = 0;

            //число слов и наборов
            if (wordsIndexes.Count != letterIndexes.Count)
                return false;

            //получаем слова и проверяем их
            for (int i = 0; i < wordsIndexes.Count; i++)
            {
                var word = words[wordsIndexes[i]];
                numOfLetters += word.Length;

                //проверка длины
                if (word.Length != letterIndexes[i].Count)
                    return false;
            }

            //поле не квадратное
            if (!CheckSquareGround(numOfLetters))
                return false;

            if(!CheckMaxIndex(letterIndexes, in numOfLetters))
                return false;


            //проверка уникальности
            if(!CheckUniqueLetters(letterIndexes, in numOfLetters))
               return false;

            return true;
        }

        private GridFillWords TryLoadLevel(string level,  string[] words)
        {
            List<int> wordsIndexes = new List<int>(5);
            List<List<int>> letterIndexes = new List<List<int>>(5);

            //парсим на слова и индексы
            ParseLevel(level, ref wordsIndexes, ref letterIndexes);

            if (!CheckLevel(wordsIndexes, letterIndexes, words, out int numOfLetters))
                return null;

            int size = (int)Mathf.Sqrt(numOfLetters);

            var output = new GridFillWords(new Vector2Int(size, size));
            for (int i = 0; i < letterIndexes.Count; i++)
                for (int j = 0; j < letterIndexes[i].Count; j++)
                {
                    var ind = letterIndexes[i][j];
                    output.Set(ind / size, ind % size, new CharGridModel(words[i][j]));
                }


            return output;
        }
    }
}