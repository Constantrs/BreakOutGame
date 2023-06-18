using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace BreakoutProject
{
    namespace Utility
    {
        /// <Summary>
        /// CSV�t�@�C���ǂݍ��݃��W���[��
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
                // reader.Peaek��-1�ɂȂ�܂�
                while (reader.Peek() != -1)
                {
                    // ��s���ǂݍ���
                    string line = reader.ReadLine();
                    if (count != 0)
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