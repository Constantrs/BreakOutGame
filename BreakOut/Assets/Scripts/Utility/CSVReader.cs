using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace BreakoutProject
{
    namespace Utility
    {
        /// <Summary>
        /// CSVファイル読み込みモジュール
        /// </Summary>
        public class CSVReader
        {
            private TextAsset _csvFile;

            public List<string> LoadCSVFile(string filename)
            {
                List<string> datas = new List<string>();
                _csvFile = Resources.Load(filename) as TextAsset;
                StringReader reader = new StringReader(_csvFile.text);

                if (reader != null && _csvFile.text.Length == 0)
                {
                    return datas;
                }

                int count = 0;
                // reader.Peaekが-1になるまで
                while (reader.Peek() != -1)
                {
                    // 一行ずつ読み込み
                    string line = reader.ReadLine();
                    // 0 - 1行追加しない
                    if (count > 1)
                    {
                        datas.Add(line);
                        Debug.Log(line);
                    }
                    count++;
                }

                return datas;
            }
        }
    }

}