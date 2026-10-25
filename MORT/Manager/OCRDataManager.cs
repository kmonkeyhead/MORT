using CloudVision;
using MORT.Model.OCR;
using MORT.OcrApi.OneOcr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using Windows.Media.Ocr;

namespace MORT
{
    public struct WinOCRResultData
    {
        public bool isEmpty;
        public int lineCount;       //라인 수.
        public string[] words;      //모든 문장.
        public double[] x;             //x값들
        public double[] y;             //y값들
        public double[] sizeX;         //size x;
        public double[] sizeY;         //size y;
        public int[] wordCounts;    //각 라인마다 워드 수.
        public double angle;
        public int wordsIndex;      //쓰이나??
    }

    public struct OcrResult
    {
        public bool isEmpty;
        public int lineCount;       //라인 수.
        public string[] words;      //모든 문장.
        public double[] x;             //x값들
        public double[] y;             //y값들
        public double[] sizeX;         //size x;
        public double[] sizeY;         //size y;
        public int[] wordCounts;    //각 라인마다 워드 수.
        public double angle;
        public int wordsIndex;      //쓰이나???


        public OcrResult(WinOCRResultData data)
        {
            isEmpty = data.isEmpty;
            lineCount = data.lineCount;
            words = data.words;
            x = data.x;
            y = data.y;
            sizeX = data.sizeX;
            sizeY = data.sizeY;
            wordCounts = data.wordCounts;
            angle = data.angle;
            wordsIndex = data.wordsIndex;
        }

        public OcrResult(GoogleOcrResult data)
        {
            isEmpty = data.isEmpty;
            lineCount = data.lineCount;
            words = data.words;
            x = data.x;
            y = data.y;
            sizeX = data.sizeX;
            sizeY = data.sizeY;
            wordCounts = data.wordCounts;
            angle = 0;
            wordsIndex = data.wordsIndex;
        }

        public OcrResult(EasyOcrResultModel data)
        {
            isEmpty = data.IsEmpty;
            lineCount = data.LineCount;
            words = data.Words;
            x = data.X;
            y = data.Y;
            sizeX = data.SizeX;
            sizeY = data.SizeY;
            wordCounts = data.WordCounts;
            angle = 0;
            wordsIndex = data.WordsIndex;
        }

        public OcrResult(OneOcr.Line[] lineList)
        {
            // 초기화
            if(lineList == null || lineList.Length == 0)
            {
                isEmpty = true;
                lineCount = 0;
                words = Array.Empty<string>();
                x = Array.Empty<double>();
                y = Array.Empty<double>();
                sizeX = Array.Empty<double>();
                sizeY = Array.Empty<double>();
                wordCounts = Array.Empty<int>();
                angle = 0;
                wordsIndex = 0;
                return;
            }

            lineCount = lineList.Length;

            var allWords = new List<string>();
            var allX = new List<double>();
            var allY = new List<double>();
            var allSizeX = new List<double>();
            var allSizeY = new List<double>();
            var counts = new List<int>();

            // 각 Line의 Word 배열을 플래튼화하여 OcrResult 필드에 매핑
            foreach(var line in lineList)
            {
                if(line == null)
                {
                    counts.Add(0);
                    continue;
                }

                var wordArray = line.Words;
                if(wordArray != null && wordArray.Length > 0)
                {
                    counts.Add(wordArray.Length);
                    foreach(var w in wordArray)
                    {
                        if(w == null)
                        {
                            allWords.Add(string.Empty);
                            allX.Add(0);
                            allY.Add(0);
                            allSizeX.Add(0);
                            allSizeY.Add(0);
                            continue;
                        }

                        allWords.Add(w.Text ?? string.Empty);

                        // 위치는 원래 방식 유지 (X1,Y1을 기준으로)
                        allX.Add(Math.Min(Math.Min(w.X1, w.X2), Math.Min(w.X3, w.X4)));
                        allY.Add(Math.Min(Math.Min(w.Y1, w.Y2), Math.Min(w.Y3, w.Y4)));

                        // 너비/높이는 네 점의 min/max로 계산하여 음수 방지
                        float minX = Math.Min(Math.Min(w.X1, w.X2), Math.Min(w.X3, w.X4));
                        float maxX = Math.Max(Math.Max(w.X1, w.X2), Math.Max(w.X3, w.X4));
                        float minY = Math.Min(Math.Min(w.Y1, w.Y2), Math.Min(w.Y3, w.Y4));
                        float maxY = Math.Max(Math.Max(w.Y1, w.Y2), Math.Max(w.Y3, w.Y4));

                        double width = Math.Max(0.0f, maxX - minX);
                        double height = Math.Max(0.0f, maxY - minY);

                        allSizeX.Add(width);
                        allSizeY.Add(height);
                    }
                }
                else
                {
                    // Word 정보가 없으면 line.Text + line bounding box를 단일 항목으로 사용 (폴백)
                    counts.Add(1);
                    allWords.Add(line.Text ?? string.Empty);
                    allX.Add(Math.Min(Math.Min(line.X1, line.X2), Math.Min(line.X3, line.X4)));
                    allY.Add(Math.Min(Math.Min(line.Y1, line.Y2), Math.Min(line.Y3, line.Y4)));

                    float minX = Math.Min(Math.Min(line.X1, line.X2), Math.Min(line.X3, line.X4));
                    float maxX = Math.Max(Math.Max(line.X1, line.X2), Math.Max(line.X3, line.X4));
                    float minY = Math.Min(Math.Min(line.Y1, line.Y2), Math.Min(line.Y3, line.Y4));
                    float maxY = Math.Max(Math.Max(line.Y1, line.Y2), Math.Max(line.Y3, line.Y4));

                    double width = Math.Max(0.0f, maxX - minX);
                    double height = Math.Max(0.0f, maxY - minY);

                    allSizeX.Add(width);
                    allSizeY.Add(height);
                }
            }

            // 결과 할당
            isEmpty = allWords.Count == 0;
            words = allWords.ToArray();
            x = allX.ToArray();
            y = allY.ToArray();
            sizeX = allSizeX.ToArray();
            sizeY = allSizeY.ToArray();
            wordCounts = counts.ToArray();
            angle = 0;
            wordsIndex = words.Length;
        }

    }

    public struct OCRResultData
    {
        public int ocrIndex;
        public Rectangle rect;
        public string ocrData;
        public string resultData;

        public bool isAlreadyUseDB;
    }


    public class OCRDataManager
    {
        public enum WordAngleType
        {
            Horizontal, Vertical,
        }

        /// <summary>
        /// 문장 데이터.
        /// </summary>
        public class TransData
        {
            public int index;
            //public int ocr;
            public string trans = "";

            public bool isInsert = false;
            public bool TitleData = false;
            public List<LineData> lineDataList = new List<LineData>();
            public Rectangle lineRect = new Rectangle();
            public Rectangle SourceRect = new Rectangle();
            public Rectangle ViewRect = new Rectangle();
            public Rectangle ContentRect = new Rectangle();
            public WordAngleType angleType;

            public bool CheckIsSameLine(LineData lineData, bool mergeLine)
            {
                bool isSame = false;

                if(lineDataList.Count > 0)
                {
                    bool IsIntersects = GetIsIntersectsWith(lineDataList[lineDataList.Count - 1], lineData, mergeLine);


                    if(IsIntersects)
                    {
                        isSame = true;
                    }
                }

                return isSame;
            }

        }


        /// <summary>
        /// 줄 데이터.
        /// </summary>
        public class LineData
        {
            public int groupIndex;
            public string lineString;
            public string transString = "";
            public List<string> wordList = new List<string>();
            public List<string> transWordList = new List<string>();
            public Rectangle lineRect = new Rectangle();
            public List<Rectangle> wordRectList = new List<Rectangle>();

            public WordAngleType angleType;


            public bool GetIsEndLine()
            {
                if(string.IsNullOrWhiteSpace(lineString))
                {
                    return false;
                }

                ReadOnlySpan<char> closingCharacters = "\"'\u201d\u2019\u300d\u300f\u3011)\u300b";
                ReadOnlySpan<char> line = lineString.AsSpan().TrimEnd();

                while(line.Length > 0 && closingCharacters.Contains(line[line.Length - 1]))
                {
                    line = line[..^1].TrimEnd();
                }

                if(line.Length == 0)
                {
                    return false;
                }

                return line[line.Length - 1] is '.' or '?' or '!'
                    or '\u3002' or '\uff1f' or '\uff01';
            }
        }

        public class ResultData
        {
            private const double FinalMergeFontRatio = 1.2;
            private const string StrongListMarkers = "\u2022\u25cf\u25cb\u25e6\u25aa\u25a0\u2023\u2043\u00b7\u30fb\uff65";

            public int Index;
            //TODO : 미리 다 해놓아야 한다.
            public bool SnapShot;
            public List<LineData> LineDataList = new List<LineData>();
            public List<TransData> TransDataList = new List<TransData>();
            public Rectangle ResultRect;
            public MORT.Model.OverlayBlurFrame BlurFrame;
            internal System.Drawing.Size OcrImageSize;
            public bool UseAutoColor { get; private set; }
            public List<(Color Font, Color BackGround)> AutoColor { get; private set; } = new();

            //public string ocrString = "";
            public string TransString = "";

            public void AddAutoColor(Color fontColor, Color backGroundColor)
            {
                AutoColor.Add(new(fontColor, backGroundColor));
                UseAutoColor = true;
            }

            public void ReplaceAutoColor(ResultData source)
            {
                AutoColor.Clear();
                AutoColor.AddRange(source.AutoColor);
                UseAutoColor = source.UseAutoColor;
            }

            public (Color Font, Color BackGround) GetAutoColor(int index)
            {
                if(AutoColor.Count > index)
                {
                    return AutoColor[index];
                }
                else
                {
                    Util.ShowLog("AutoColor is not exist. index = " + index + " / " + AutoColor.Count);
                    return (Color.White, Color.Black);
                }
            }

            public bool TryGetAutoColor(int index, out (Color Font, Color BackGround) color)
            {
                if(index >= 0 && index < AutoColor.Count)
                {
                    color = AutoColor[index];
                    return true;
                }

                Util.ShowLog("AutoColor is not exist. index = " + index + " / " + AutoColor.Count);
                color = default;
                return false;
            }

            public string GetOCR()
            {
                string ocr = "";

                for(int i = 0; i < LineDataList.Count; i++)
                {
                    for(int j = 0; j < LineDataList[i].wordList.Count; j++)
                    {
                        ocr += LineDataList[i].wordList[j] + " ";
                    }
                }

                return ocr;
            }

            public string GetTrans()
            {
                string ocr = "";
                for(int i = 0; i < TransDataList.Count; i++)
                {
                    ocr += TransDataList[i].trans + " ";
                }

                return GetOCR();
            }

            public List<string> GetOcrText()
            {
                List<string> list = new List<string>();
                for(int i = 0; i < TransDataList.Count; i++)
                {
                    string text = "";
                    for(int j = 0; j < TransDataList[i].lineDataList.Count; j++)
                    {

                        text += TransDataList[i].lineDataList[j].lineString;
                    }

                    list.Add(text);
                }

                return list;
            }

            private static bool IsTitleData(LineData lineData, bool removeSpaceMode)
            {
                int targetCount = removeSpaceMode ? 6 : 10;
                targetCount -= lineData.angleType == WordAngleType.Vertical ? 3 : 0;
                int charCount = lineData.wordList.Sum(word => word.Length);
                if(charCount <= targetCount)
                {
                    return true;
                }

                if(removeSpaceMode)
                {
                    return false;
                }

                int wordCount = lineData.wordList.Count;
                return  wordCount <= 3;
            }

            public void InitLine(bool mergeLine, bool removeSpaceMode)
            {
                TransDataList.Clear();
                //면적이 없는 줄은 독립 블록이나 병합 후보로 사용하지 않는다.
                LineDataList.RemoveAll(line => line.lineRect.Width <= 0 || line.lineRect.Height <= 0);

                if(!mergeLine || Form1.IsDebugTransOneLine)
                {
                    foreach(var lineData in LineDataList)
                    {
                        TransDataList.Add(CreateTransData(lineData));
                    }
                }
                else
                {
                    InitSpatialLines(removeSpaceMode);
                }

                UpdateTransRectangles();
            }

            private void InitSpatialLines(bool removeSpaceMode)
            {
                int count = LineDataList.Count;
                int[] parent = Enumerable.Range(0, count).ToArray();

                for(int i = 0; i < count; i++)
                {
                    for(int j = i + 1; j < count; j++)
                    {
                        if(AreSpatiallyAdjacent(LineDataList[i], LineDataList[j]))
                        {
                            Union(parent, i, j);
                        }
                    }
                }

                var components = Enumerable.Range(0, count)
                    .GroupBy(index => Find(parent, index))
                    .Select(group => group.Select(index => LineDataList[index]).ToList())
                    .ToList();

                foreach(var component in components)
                {
                    SortComponent(component);
                }
                components.Sort(CompareComponents);

                foreach(var component in components)
                {
                    bool hasListContext = HasListContext(component);
                    TransData current = null;
                    LineData previous = null;

                    for(int i = 0; i < component.Count; i++)
                    {
                        LineData lineData = component[i];
                        LineData next = i + 1 < component.Count ? component[i + 1] : null;
                        bool isListItem = IsListItem(lineData, hasListContext);
                        bool isTitle = !isListItem && (IsExplicitTitle(lineData)
                            || (i == 0 && IsContextTitle(lineData, next, removeSpaceMode)));

                        if(isListItem)
                        {
                            TransData listItem = CreateTransData(lineData);
                            TransDataList.Add(listItem);
                            LogMergeDecision(lineData, "list marker boundary");
                            current = null;
                            previous = lineData;
                            continue;
                        }

                        if(isTitle)
                        {
                            current = CreateTransData(lineData);
                            current.TitleData = true;
                            TransDataList.Add(current);
                            current = null;
                            previous = lineData;
                            continue;
                        }

                        if(current == null || previous == null || previous.GetIsEndLine()
                            || !CanAppendToBlock(current, previous, lineData))
                        {
                            current = CreateTransData(lineData);
                            TransDataList.Add(current);
                        }
                        else
                        {
                            current.lineDataList.Add(lineData);
                        }

                        previous = lineData;
                        if(lineData.GetIsEndLine())
                        {
                            current = null;
                        }
                    }
                }
            }

            private static bool CanAppendToBlock(TransData current, LineData previous, LineData candidate)
            {
                if(!AreSpatiallyAdjacent(previous, candidate))
                {
                    LogMergeDecision(candidate, "not spatially adjacent");
                    return false;
                }

                double candidateSize = GetFontSize(candidate);
                var blockSizes = current.lineDataList
                    .Select(GetFontSize)
                    .Where(size => size > 0)
                    .OrderBy(size => size)
                    .ToList();
                if(candidateSize <= 0 || blockSizes.Count == 0)
                {
                    LogMergeDecision(candidate, "invalid font size");
                    return false;
                }

                int middle = blockSizes.Count / 2;
                double blockMedian = blockSizes.Count % 2 == 1
                    ? blockSizes[middle]
                    : (blockSizes[middle - 1] + blockSizes[middle]) / 2.0;
                double medianRatio = GetFontRatio(candidateSize, blockMedian);
                if(medianRatio > FinalMergeFontRatio)
                {
                    LogMergeDecision(candidate, $"font/median ratio {medianRatio:0.00}");
                    return false;
                }

                double minimum = Math.Min(candidateSize, blockSizes[0]);
                double maximum = Math.Max(candidateSize, blockSizes[blockSizes.Count - 1]);
                double bandRatio = GetFontRatio(maximum, minimum);
                if(bandRatio > FinalMergeFontRatio)
                {
                    LogMergeDecision(candidate, $"font band ratio {bandRatio:0.00}");
                    return false;
                }

                return true;
            }

            private static double GetFontRatio(double first, double second)
            {
                double minimum = Math.Min(first, second);
                return minimum <= 0 ? double.MaxValue : Math.Max(first, second) / minimum;
            }

            private static bool HasListContext(List<LineData> component)
            {
                int markerCandidates = 0;
                foreach(LineData lineData in component)
                {
                    ReadOnlySpan<char> text = (lineData.lineString ?? string.Empty).AsSpan().TrimStart();
                    if(IsStrongListMarker(text) || IsExplicitWeakListMarker(text) || IsNumberedListMarker(text))
                    {
                        return true;
                    }

                    if(IsWeakListMarkerCandidate(text))
                    {
                        markerCandidates++;
                    }
                }

                return markerCandidates >= 2;
            }

            private static bool IsListItem(LineData lineData, bool hasListContext)
            {
                ReadOnlySpan<char> text = (lineData.lineString ?? string.Empty).AsSpan().TrimStart();
                return IsStrongListMarker(text)
                    || IsNumberedListMarker(text)
                    || IsExplicitWeakListMarker(text)
                    || (hasListContext && IsWeakListMarkerCandidate(text));
            }

            private static bool IsStrongListMarker(ReadOnlySpan<char> text)
            {
                return text.Length > 1 && StrongListMarkers.AsSpan().Contains(text[0]);
            }

            private static bool IsWeakListMarkerCandidate(ReadOnlySpan<char> text)
            {
                return text.Length > 1 && text[0] is '-' or '*' or '.';
            }

            private static bool IsExplicitWeakListMarker(ReadOnlySpan<char> text)
            {
                return IsWeakListMarkerCandidate(text) && char.IsWhiteSpace(text[1]);
            }

            private static bool IsNumberedListMarker(ReadOnlySpan<char> text)
            {
                if(text.Length < 3)
                {
                    return false;
                }

                int index = 0;
                bool wrapped = text[index] == '(';
                if(wrapped)
                {
                    index++;
                }

                int tokenStart = index;
                while(index < text.Length && index - tokenStart < 3 && char.IsLetterOrDigit(text[index]))
                {
                    index++;
                }

                if(index == tokenStart)
                {
                    return false;
                }

                if(wrapped)
                {
                    if(index >= text.Length || text[index] != ')')
                    {
                        return false;
                    }
                    index++;
                }
                else
                {
                    if(index >= text.Length || text[index] is not ('.' or ')'))
                    {
                        return false;
                    }
                    index++;
                }

                return index < text.Length && char.IsWhiteSpace(text[index])
                    && text[index..].TrimStart().Length > 0;
            }

            private static void LogMergeDecision(LineData lineData, string reason)
            {
                if(Form1.IsDebugShowWordArea)
                {
                    Util.ShowLog($"Overlay merge split: {reason}, font={GetFontSize(lineData)}, text={lineData.lineString}");
                }
            }

            private static TransData CreateTransData(LineData lineData)
            {
                var transData = new TransData
                {
                    isInsert = true,
                    angleType = lineData.angleType,
                };
                transData.lineDataList.Add(lineData);
                return transData;
            }

            private static bool IsExplicitTitle(LineData lineData)
            {
                string text = (lineData.lineString ?? string.Empty).Trim();
                if(text.Length == 0)
                {
                    return false;
                }

                bool wrapped = (text.StartsWith("[") && text.EndsWith("]"))
                    || (text.StartsWith("\u3010") && text.EndsWith("\u3011"))
                    || (text.StartsWith("<") && text.EndsWith(">"));
                return wrapped || text.EndsWith(":") || text.EndsWith("\uff1a");
            }

            private static bool IsContextTitle(LineData lineData, LineData next, bool removeSpaceMode)
            {
                if(next == null || lineData.angleType != next.angleType || !IsTitleData(lineData, removeSpaceMode))
                {
                    return false;
                }

                int currentLength = GetCharacterCount(lineData);
                int nextLength = GetCharacterCount(next);
                return currentLength > 0 && nextLength >= Math.Ceiling(currentLength * 1.5);
            }

            private static int GetCharacterCount(LineData lineData)
            {
                return (lineData.lineString ?? string.Empty).Count(character => !char.IsWhiteSpace(character));
            }

            private static void SortComponent(List<LineData> component)
            {
                if(component.Count == 0)
                {
                    return;
                }

                if(component[0].angleType == WordAngleType.Vertical)
                {
                    component.Sort((left, right) =>
                    {
                        int column = right.lineRect.Right.CompareTo(left.lineRect.Right);
                        return column != 0 ? column : left.lineRect.Top.CompareTo(right.lineRect.Top);
                    });
                }
                else
                {
                    component.Sort((left, right) =>
                    {
                        int row = left.lineRect.Top.CompareTo(right.lineRect.Top);
                        return row != 0 ? row : left.lineRect.Left.CompareTo(right.lineRect.Left);
                    });
                }
            }

            private static int CompareComponents(List<LineData> left, List<LineData> right)
            {
                Rectangle leftRect = GetComponentRect(left);
                Rectangle rightRect = GetComponentRect(right);
                int row = leftRect.Top.CompareTo(rightRect.Top);
                if(row != 0)
                {
                    return row;
                }

                return left[0].angleType == WordAngleType.Vertical
                    ? rightRect.Right.CompareTo(leftRect.Right)
                    : leftRect.Left.CompareTo(rightRect.Left);
            }

            private static Rectangle GetComponentRect(List<LineData> component)
            {
                Rectangle rect = component[0].lineRect;
                for(int i = 1; i < component.Count; i++)
                {
                    rect = Rectangle.Union(rect, component[i].lineRect);
                }
                return rect;
            }

            private static bool AreSpatiallyAdjacent(LineData left, LineData right)
            {
                if(left.angleType != right.angleType)
                {
                    return false;
                }

                double leftSize = GetFontSize(left);
                double rightSize = GetFontSize(right);
                double maximum = Math.Max(leftSize, rightSize);
                double minimum = Math.Min(leftSize, rightSize);
                if(minimum <= 0 || maximum / minimum > 1.3)
                {
                    return false;
                }

                double size = (leftSize + rightSize) / 2.0;
                Rectangle a = left.lineRect;
                Rectangle b = right.lineRect;

                if(left.angleType == WordAngleType.Horizontal)
                {
                    bool crossAxis = GetOverlapRatio(a.Left, a.Right, b.Left, b.Right) >= 0.25
                        || Math.Abs(a.Left - b.Left) <= size * 2;
                    return GetAxisGap(a.Top, a.Bottom, b.Top, b.Bottom) <= size * 1.25
                        && crossAxis;
                }

                bool verticalCrossAxis = GetOverlapRatio(a.Top, a.Bottom, b.Top, b.Bottom) >= 0.25
                    || Math.Abs(a.Top - b.Top) <= size * 2;
                return GetAxisGap(a.Left, a.Right, b.Left, b.Right) <= size * 1.25
                    && verticalCrossAxis;
            }

            private static int GetAxisGap(int firstStart, int firstEnd, int secondStart, int secondEnd)
            {
                return Math.Max(0, Math.Max(firstStart, secondStart) - Math.Min(firstEnd, secondEnd));
            }

            private static double GetOverlapRatio(int firstStart, int firstEnd, int secondStart, int secondEnd)
            {
                int overlap = Math.Max(0, Math.Min(firstEnd, secondEnd) - Math.Max(firstStart, secondStart));
                int shortLength = Math.Max(1, Math.Min(firstEnd - firstStart, secondEnd - secondStart));
                return overlap / (double)shortLength;
            }

            private static int Find(int[] parent, int index)
            {
                while(parent[index] != index)
                {
                    parent[index] = parent[parent[index]];
                    index = parent[index];
                }
                return index;
            }

            private static void Union(int[] parent, int left, int right)
            {
                int leftRoot = Find(parent, left);
                int rightRoot = Find(parent, right);
                if(leftRoot != rightRoot)
                {
                    parent[rightRoot] = leftRoot;
                }
            }

            private void UpdateTransRectangles()
            {
                foreach(var transData in TransDataList)
                {
                    if(transData.lineDataList.Count == 0)
                    {
                        continue;
                    }

                    Rectangle rect = transData.lineDataList[0].lineRect;
                    for(int i = 1; i < transData.lineDataList.Count; i++)
                    {
                        rect = Rectangle.Union(rect, transData.lineDataList[i].lineRect);
                    }

                    transData.lineRect = rect;
                    transData.SourceRect = rect;
                    transData.ViewRect = rect;
                    transData.ContentRect = rect;
                }
            }

            private void InitLineLegacy(bool mergeLine, bool removeSpaceMode)
            {
                TransData transData = null;
                for(int i = 0; i < LineDataList.Count; i++)
                {
                    var lineData = LineDataList[i];
                    if(lineData.angleType == WordAngleType.Horizontal)
                    {
                        var rect = RecognizeRect(lineData.lineString, lineData.lineRect);
                        lineData.lineRect = rect;
                    }
 
                    bool isNew = false;

                    if(transData == null)
                    {
                        isNew = true;
                    }

                    if(isNew)
                    {
                        transData = new TransData();
                        transData.lineDataList.Add(LineDataList[i]);
                        transData.isInsert = true;
                        transData.angleType = LineDataList[i].angleType;
                        this.TransDataList.Add(transData);

                        //첫 시작이 특정 수 미만이면 바로 다음으로 넘어간다
                        //TODO : 인접한 문구가 있는지는 검사해야한다 
                        if(IsTitleData(LineDataList[i], removeSpaceMode))
                        {
                            transData.TitleData = true;
                            transData = null;
                        }
                    }
                    else
                    {

                        if(!Form1.IsDebugTransOneLine && transData != null && transData.CheckIsSameLine(LineDataList[i], mergeLine))
                        {
                            //같은 라인이다.

                            bool isEnd = LineDataList[i].GetIsEndLine();
                            transData.lineDataList.Add(LineDataList[i]);

                            if(isEnd)
                            {
                                transData = null;
                            }
                        }
                        else
                        {
                            //같은 라인이 아니다.
                            transData = new TransData();
                            transData.lineDataList.Add(LineDataList[i]);
                            transData.isInsert = true;
                            transData.angleType = LineDataList[i].angleType;

                            this.TransDataList.Add(transData);

                            //첫 시작이 특정 수 미만이면 바로 다음으로 넘어간다
                            if(IsTitleData(LineDataList[i], removeSpaceMode))
                            {
                                transData.TitleData = true;
                                transData = null;
                            }
                        }
                    }
                }

                for(int i = 0; i < TransDataList.Count; i++)
                {
                    TransDataList[i].lineRect = new Rectangle();

                    if(TransDataList[i].lineDataList.Count > 0)
                    {
                        var rect = TransDataList[i].lineDataList[0].lineRect;
                        for(int j = 1; j < TransDataList[i].lineDataList.Count; j++)
                        {
                            rect = Rectangle.Union(rect, TransDataList[i].lineDataList[j].lineRect);
                        }

                        TransDataList[i].lineRect = rect;
                    }
                }
            }

            /// <summary>
            /// 번역 결과 초기화.
            /// </summary>
            /// <param name="transString"></param>
            public void ApplyTransResult(string transString, SettingManager.TransType transType)
            {
                this.TransString = transString;

                string[] words = Util.GetSpliteByToken(this.TransString, transType);

                for(int i = 0; i < TransDataList.Count && i < words.Length; i++)
                {
                    TransDataList[i].trans = words[i];
                }
            }

        }
        private static OCRDataManager instance;
        public static OCRDataManager Instace
        {
            get
            {
                if(instance == null)
                {
                    instance = new OCRDataManager();
                }
                return instance;
            }
        }
        private List<OCRResultData> resultList = new List<OCRResultData>(); //안 쓰임.
        private List<ResultData> dataList = new List<ResultData>();
        public bool MergeLine { get; set; } = false;


        /// <summary>
        /// 목록만 복사해서 넘긴다. 원소는 그대로 공유한다.
        /// 번역 스레드가 다음 회차에 ClearData 로 내부 목록을 비우기 때문에,
        /// 내부 목록을 그대로 넘기면 번역창이 순회하는 도중에 비어버린다.
        /// </summary>
        public List<ResultData> GetData()
        {
            List<ResultData> list = new List<ResultData>();
            for(int i = 0; i < dataList.Count; i++)
            {
                list.Add(dataList[i]);
            }
            return list;
        }

        public ResultData GetData(int index)
        {
            ResultData data = null;

            for(int i = 0; i < dataList.Count; i++)
            {
                if(dataList[i].Index == index)
                {
                    data = dataList[i];
                }
            }

            return data;
        }
        public static int GetFontSize(LineData data)
        {
            var sizes = data.wordRectList
                .Where(rect => rect.Width > 0 && rect.Height > 0)
                .Select(rect => Math.Min(rect.Width, rect.Height))
                .OrderBy(size => size)
                .ToList();

            if(sizes.Count == 0)
            {
                return 10;
            }

            int middle = sizes.Count / 2;
            return sizes.Count % 2 == 1
                ? sizes[middle]
                : Math.Max(1, (sizes[middle - 1] + sizes[middle]) / 2);
        }

        private static (bool isxHeight, bool hasAcent, bool hasHarfAcent, bool hasDecent) GetTextType(string text)
        {
            // abcdefghijklmnopqrstuvwxyz
            // ABCDEFGHIJKLMNOPQRSTUVWXYZ
            var isxHeight = Contains(text, "acemnosuvwxz<>+=");
            var hasAcent = Contains(text, "ABCDEFGHIJKLMNOPQRSTUVWXYZbdfhijkl!\"#$%&'()|/[]{}@");
            var hasHarfAcent = Contains(text, "t^");
            var hasDecent = Contains(text, "gjpqy()|[]{}@");
            return (isxHeight, hasAcent, hasHarfAcent, hasDecent);
        }

        private static bool Contains(string text, string target)
        {
            ReadOnlySpan<char> te = text;
            ReadOnlySpan<char> ta = target;
            return te.ContainsAny(ta);
        }

        public static Rectangle RecognizeRect(string text, Rectangle rectangle)
        {
            var (isxHeight, hasAcent, hasHarfAcent, hasDecent) = GetTextType(text);

            double height = rectangle.Height;
            double y = rectangle.Y;

            y -= (hasAcent, hasHarfAcent) switch
            {
                (true, _) => 0,
                (false, true) => (int)(height * .1),
                (false, false) => height * .2,
            };

            height = CorrectHeight(height, isxHeight, hasAcent, hasHarfAcent, hasDecent);
            return new(rectangle.X, (int)y, rectangle.Width, (int)height);
        }

        private static double CorrectHeight(double height, bool isxHeight, bool hasAcent, bool hasHarfAcent, bool hasDecent)
    => (isxHeight, hasAcent, hasHarfAcent, hasDecent) switch
    {
        (true, true, _, true) => height,
        (true, true, _, false) => height * 1.2,
        (true, false, true, true) => height * (1 + .1 + .0),
        (true, false, false, true) => height * (1 + .2 + .0),
        (true, false, true, false) => height * (1 + .1 + .2),
        (true, false, false, false) => height * (1 + .2 + .2),
        (false, _, _, true) => height,
        (false, _, _, false) => height * 1.2,
    };


        public static bool GetIsIntersectsWith(LineData beforeData, LineData data, bool mergeLine)
        {
            bool isIntersect = false;

            if(beforeData.angleType == data.angleType)
            {
                Rectangle rect1 = new Rectangle(beforeData.lineRect.X, beforeData.lineRect.Y, beforeData.lineRect.Width, beforeData.lineRect.Height);
                int beforeFontSize = GetFontSize(beforeData);
                int fontSize = GetFontSize(data);

                int diff = Math.Abs(beforeFontSize - fontSize);
                float percent = (float)(diff) / (float)fontSize;
                //Util.ShowLog("Before : " + beforeFontSize + " / current : " + fontSize + " / diff : " + diff + " / percent : " + (float)percent);
                //폰트 크기가 90% 이상 차이면 안 합니다
                if(percent > 0.9f)
                {
                    return false;
                }

                //가로 처리
                if(beforeData.angleType == WordAngleType.Horizontal)
                {
                    Rectangle rect2 = rect1;
                    rect2.Width += (int)(beforeFontSize * 4f);
                    isIntersect = rect2.IntersectsWith(data.lineRect);

                    if(!isIntersect && mergeLine)
                    {
                        //y 축을 먼저 검사한다
                        float yGap = Math.Abs((beforeData.lineRect.Y + beforeData.lineRect.Height) - data.lineRect.Y);
                        float lThre = fontSize * 1.1f;
                        if(yGap >= lThre)
                        {
                            return false;
                        }
                        
                        // x 축 검사 - 두 라인의 시작점 차이가 폰트 사이즈의 4배 이상이면 합치지 않음
                        float xGap = Math.Abs(beforeData.lineRect.X - data.lineRect.X);
                        float xThre = fontSize * 4f;
                        if (xGap >= xThre)
                        {
                            return false;
                        }

                        return true;
                    }
                    else
                    {
                        Util.ShowLog("Is Splite line?????????");
                    }

                }
                //세로 처리
                else if(mergeLine)
                {
                    int adjust = (int)(beforeFontSize * 0.8f);

                    rect1.Width += adjust;
                    rect1.X -= adjust;
                    isIntersect = rect1.IntersectsWith(data.lineRect);
                }
            }


            return isIntersect;
        }

        public void ClearData()
        {
            if(dataList == null)
            {
                dataList = new List<ResultData>();
            }
            else
            {
                dataList.Clear();
            }
        }

        public ResultData AddData(OcrResult data, int index, bool snapShot, bool removeSpace)
        {
            ResultData resultData = new ResultData();
            resultData.Index = index;
            resultData.SnapShot = snapShot;

            //Util.ShowLog("line = " + point.lineCount);
            int count = 0;
            for(int i = 0; i < data.lineCount; i++)
            {
                LineData lineData = new LineData();
                string lineString = "";
                //Util.ShowLog("----line start----");

                for(int j2 = 0; j2 < data.wordCounts[i]; j2++)
                {
                    Rectangle rect = CreateOutwardRectangle(data.x[count], data.y[count], data.sizeX[count], data.sizeY[count]);
                    if(rect.Width <= 0 || rect.Height <= 0)
                    {
                        count++;
                        continue;
                    }

                    lineString += data.words[count] + " ";
                    lineData.wordList.Add(data.words[count]);
                    lineData.wordRectList.Add(rect);

                    count++;
                }
                if(lineData.wordRectList.Count == 0)
                {
                    continue;
                }
                Rectangle lineRect = new Rectangle();
                //줄 처리.
                if(lineData.wordRectList.Count > 1)
                {
                    lineRect = lineData.wordRectList[0];
                    for(int j = 1; j < lineData.wordRectList.Count; j++)
                    {
                        lineRect = Rectangle.Union(lineRect, lineData.wordRectList[j]);
                    }
                    lineData.lineRect = lineRect;
                }
                else if(lineData.wordRectList.Count == 1)
                {
                    lineRect = lineData.wordRectList[0];
                    lineData.lineRect = lineRect;
                }

                lineData.lineString = lineString;
                resultData.LineDataList.Add(lineData);

                if(lineRect.Height > lineRect.Width * 1.5f)
                {
                    lineData.angleType = WordAngleType.Vertical;
                }
                else
                {
                    lineData.angleType = WordAngleType.Horizontal;
                }
            }

            //전체 영역 처리.
            if(resultData.LineDataList.Count == 1)
            {
                resultData.ResultRect = resultData.LineDataList[0].lineRect;
            }
            else if(resultData.LineDataList.Count > 1)
            {
                resultData.ResultRect = resultData.LineDataList[0].lineRect;
                for(int i = 1; i < resultData.LineDataList.Count; i++)
                {
                    resultData.ResultRect = Rectangle.Union(resultData.ResultRect, resultData.LineDataList[i].lineRect);
                }
            }

            resultData.InitLine(MergeLine, removeSpace);
            dataList.Add(resultData);


            return resultData;
        }

        private static Rectangle CreateOutwardRectangle(double x, double y, double width, double height)
        {
            //소수 좌표의 바깥 반올림으로 크기 0이 1px 영역으로 되살아나지 않도록 먼저 검사한다.
            if(width <= 0 || height <= 0)
            {
                return Rectangle.Empty;
            }

            int left = (int)Math.Floor(x);
            int top = (int)Math.Floor(y);
            int right = (int)Math.Ceiling(x + Math.Max(0, width));
            int bottom = (int)Math.Ceiling(y + Math.Max(0, height));
            return Rectangle.FromLTRB(left, top, Math.Max(left, right), Math.Max(top, bottom));
        }
    }



}
