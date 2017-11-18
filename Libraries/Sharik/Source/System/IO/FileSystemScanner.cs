// Free open-source Sharik library - http://code.google.com/p/sharik/

using System;
using System.Collections.Generic;
using System.IO;

namespace Sharik.IO
{
    public class FileSystemScanner
    {
        public enum Scope { Everything = 0, DirectoriesOnly, FilesOnly };
        public enum Direction { Ascending = 0, Descending };
        public enum Priority { DirectoriesFirst = 0, FilesFirst, AsDefinedByGeneralOrder };
        public enum RecursePriority { ParentFirst = 0, ChildrenFirst };

        public struct Options
        {
            public Func<string, Exception, bool> ErrorHandler;
            public Scope Scope; // Everything by default
            public Direction Direction; // Ascending by default
            public Priority Priority; // DirectoriesFirst by default
            public RecursePriority RecursePriority; // ParentFirst by default
            public bool OmitIntermediateLevels; // false by default
        }

        public static IEnumerable<string> Scan(string rootPath, Options options)
        {
            return Scan(rootPath, options, -1, null, false, null, false);
        }

        public static IEnumerable<string> Scan(string rootPath, Options options, int maxLevel, string minPath, string maxPath)
        {
            return Scan(rootPath, options, maxLevel, minPath, true, maxPath, true);
        }

        public static IEnumerable<string> Scan(string rootPath, Options options, int maxLevel, string minPath, bool includingMin, string maxPath, bool includingMax)
        {
            if (minPath != null)
            {
                minPath = minPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                if (string.Compare(rootPath, 0, minPath, 0, rootPath.Length) != 0)
                    throw new ArgumentException("minPath should match rootPath");
            }
            if (maxPath != null)
            {
                maxPath = maxPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                if (string.Compare(rootPath, 0, maxPath, 0, rootPath.Length) != 0)
                    throw new ArgumentException("maxPath should match rootPath");
            }
            if (maxLevel < 0 && options.OmitIntermediateLevels)
                throw new ArgumentException("not implemented: maxLevelCount < 0 && omitIntermediateLevels");
            var c = CreateComparison(options.Direction, options.Priority);
            return ScanImpl(rootPath, options, maxLevel, minPath, includingMin, maxPath, includingMax, c.Compare, "");
        }

        // Internal

        private static IEnumerable<string> ScanImpl(string rootPath, Options options, int maxLevel,
            string minPath, bool includingMin, string maxPath, bool includingMax, Comparison<FileSystemInfo> comparison, string subRootPath)
        {
            var currentDir = Path.Combine(rootPath, subRootPath);
            var yieldDirs = options.Scope != Scope.FilesOnly && (maxLevel <= 0 || !options.OmitIntermediateLevels);
            var yieldFiles = options.Scope != Scope.DirectoriesOnly && (maxLevel <= 1 || !options.OmitIntermediateLevels);
            if (yieldDirs && options.RecursePriority == RecursePriority.ParentFirst)
                yield return currentDir;
            // Process files recursively
            if (maxLevel != 0) // -1 in case of unlimited depth
            {
                var children = GetSortedFileSystemInfos(currentDir, options.Scope, comparison, options.ErrorHandler);
                if (children != null)
                {
                    foreach (var x in children)
                    {
                        var path = Path.Combine(currentDir, x.Name);
                        var c = ComparePathWithMinMaxRange(path, x is DirectoryInfo, minPath, includingMin, maxPath, includingMax, rootPath.Length);
                        if (c == 0) // if inside the min/max range, then either do scanning recursively or yield the path
                        {
                            if (x is DirectoryInfo)
                            {
                                var t = ScanImpl(rootPath, options, maxLevel - 1, minPath, includingMin, maxPath,
                                    includingMax, comparison, Path.Combine(subRootPath, x.Name));
                                foreach (var p in t)
                                    yield return p;
                            }
                            else if (yieldFiles && x is FileInfo)
                                yield return path;
                        }
                        else if (c < 0 == (options.Direction == Direction.Ascending))
                            continue; // if (c < 0 && options.Direction == Direction.Ascending) or (c > 0 && options.Direction == Direction.Descending)
                        else
                            break; // if (c < 0 && options.Direction == Direction.Descending) or (c > 0 && options.Direction == Direction.Ascending)
                    }
                }
            }
            if (yieldDirs && options.RecursePriority == RecursePriority.ChildrenFirst)
                yield return currentDir;
        }

        private static FileSystemInfo[] GetSortedFileSystemInfos(
            string dir, Scope scope, Comparison<FileSystemInfo> comparison,
            Func<string, Exception, bool> handleException)
        {
            FileSystemInfo[] result = null;
            try
            {
                var di = new DirectoryInfo(dir);
                if (scope == Scope.DirectoriesOnly)
                    result = di.GetDirectories();
                else
                    result = di.GetFileSystemInfos();
                Array.Sort(result, comparison);
            }
            catch (Exception e)
            {
                if (handleException == null || !handleException(dir, e))
                    throw;
            }
            return result;
        }

        private static int ComparePathWithMinMaxRange(string path, bool isDir, string min, bool includingMin, string max, bool includingMax, int startFrom)
        {
            var result = 0;
            var tmin = (includingMin || isDir) ? 0 : 1;
            var tmax = (includingMax || isDir) ? 0 : -1;
            if (min != null && ComparePaths(path, min, startFrom) < tmin)
                result = -1;
            else if (max != null && ComparePaths(path, max, startFrom) > tmax)
                result = 1;
            return result;
        }

        private static int ComparePaths(string path1, string path2, int startFrom)
        {
            var length = Math.Min(path1.Length, path2.Length) - startFrom;
            var result = string.Compare(path1, startFrom, path2, startFrom, length);
            return result;
        }

        private static MultiComparison<FileSystemInfo> CreateComparison(Direction direction, Priority priority)
        {
            var result = new MultiComparison<FileSystemInfo>();
            if (priority == Priority.DirectoriesFirst)
                result.Add((x, y) => CompareFileSystemInfos(x, y));
            else if (priority == Priority.FilesFirst)
                result.Add((x, y) => -CompareFileSystemInfos(x, y));
            if (direction == Direction.Ascending)
                result.Add((x, y) => x.Name.CompareTo(y.Name));
            else
                result.Add((x, y) => -x.Name.CompareTo(y.Name));
            return result;
        }

        private static int CompareFileSystemInfos(FileSystemInfo x, FileSystemInfo y)
        {
            var xx = x is DirectoryInfo ? -1 : 1;
            var yy = y is DirectoryInfo ? -1 : 1;
            return xx - yy;
        }
    }
}
