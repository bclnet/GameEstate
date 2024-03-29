﻿using GameEstate.Explorer;
using GameEstate.Formats;
using GameEstate.Transforms;
using OpenStack.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static GameEstate.Estate;

namespace GameEstate
{
    /// <summary>
    /// AbstractPakFile
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class EstatePakFile : IDisposable
    {
        public readonly Estate Estate;
        public readonly string Game;
        public readonly string Name;
        public readonly IDictionary<Type, Func<string, string>> PathFinders = new Dictionary<Type, Func<string, string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EstatePakFile" /> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <exception cref="ArgumentNullException">filePaths
        /// or
        /// game</exception>
        public EstatePakFile(Estate estate, string game, string name)
        {
            Estate = estate ?? throw new ArgumentNullException(nameof(estate));
            Game = game ?? throw new ArgumentNullException(nameof(game));
            Name = name;
        }

        /// <summary>
        /// Is valid
        /// </summary>
        public virtual bool Valid => true;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified file path]; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool Contains(string path);

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="fileId">The fileId.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified file path]; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool Contains(int fileId);

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public abstract int Count { get; }

        /// <summary>
        /// Finds the texture.
        /// </summary>
        /// <param name="path">The texture path.</param>
        /// <returns></returns>
        public string FindPath<T>(string path)
        {
            if (PathFinders.Count != 1) return PathFinders.TryGetValue(typeof(T), out var pathFinder) ? pathFinder(path) : path;
            var first = PathFinders.First();
            return first.Key == typeof(T) || first.Key == typeof(object) ? first.Value(path) : path;
        }

        /// <summary>
        /// Loads the file data asynchronous.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="option">The option.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public abstract Task<Stream> LoadFileDataAsync(string path, DataOption option = 0, Action<FileMetadata, string> exception = null);
        /// <summary>
        /// Loads the file data asynchronous.
        /// </summary>
        /// <param name="fileId">The fileId.</param>
        /// <param name="option">The option.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public abstract Task<Stream> LoadFileDataAsync(int fileId, DataOption option = 0, Action<FileMetadata, string> exception = null);
        /// <summary>
        /// Loads the file data asynchronous.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="option">The option.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public abstract Task<Stream> LoadFileDataAsync(FileMetadata file, DataOption option = 0, Action<FileMetadata, string> exception = null);

        /// <summary>
        /// Loads the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The file path.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public abstract Task<T> LoadFileObjectAsync<T>(string path, Action<FileMetadata, string> exception = null);
        /// <summary>
        /// Loads the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileId">The fileId.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public abstract Task<T> LoadFileObjectAsync<T>(int fileId, Action<FileMetadata, string> exception = null);
        /// <summary>
        /// Loads the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">The file.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public abstract Task<T> LoadFileObjectAsync<T>(FileMetadata file, Action<FileMetadata, string> exception = null);

        /// <summary>
        /// Loads the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The file path.</param>
        /// <param name="transformTo">The transformTo.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public async Task<T> LoadFileObjectAsync<T>(string path, EstatePakFile transformTo, Action<FileMetadata, string> exception = null)
            => await TransformFileObjectAsync<T>(transformTo, await LoadFileObjectAsync<object>(path, exception));
        /// <summary>
        /// Loads the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileId">The fileId.</param>
        /// <param name="transformTo">The transformTo.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public async Task<T> LoadFileObjectAsync<T>(int fileId, EstatePakFile transformTo, Action<FileMetadata, string> exception = null)
            => await TransformFileObjectAsync<T>(transformTo, await LoadFileObjectAsync<object>(fileId, exception));
        /// <summary>
        /// Loads the object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">The file.</param>
        /// <param name="transformTo">The transformTo.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public async Task<T> LoadFileObjectAsync<T>(FileMetadata fileId, EstatePakFile transformTo, Action<FileMetadata, string> exception = null)
            => await TransformFileObjectAsync<T>(transformTo, await LoadFileObjectAsync<object>(fileId, exception));

        /// <summary>
        /// Transforms the file object asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transformTo">The transformTo.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        Task<T> TransformFileObjectAsync<T>(EstatePakFile transformTo, object source)
        {
            if (this is ITransformFileObject<T> left && left.CanTransformFileObject(transformTo, source)) return left.TransformFileObjectAsync(transformTo, source);
            else if (transformTo is ITransformFileObject<T> right && right.CanTransformFileObject(transformTo, source)) return right.TransformFileObjectAsync(transformTo, source);
            else throw new ArgumentOutOfRangeException(nameof(transformTo));
        }

        /// <summary>
        /// Gets the graphic.
        /// </summary>
        /// <value>
        /// The graphic.
        /// </value>
        public IOpenGraphic Graphic { get; internal set; }

        #region Explorer

        /// <summary>
        /// Gets the explorer item nodes.
        /// </summary>
        /// <param name="manager">The resource.</param>
        /// <returns></returns>
        public virtual Task<List<ExplorerItemNode>> GetExplorerItemNodesAsync(ExplorerManager manager)
            => throw new NotImplementedException();

        /// <summary>
        /// Gets the explorer item filters.
        /// </summary>
        /// <param name="manager">The resource.</param>
        /// <returns></returns>
        public virtual Task<List<ExplorerItemNode.Filter>> GetExplorerItemFiltersAsync(ExplorerManager manager)
            => Task.FromResult(Estate.FileManager.Filters.TryGetValue(Game, out var z) ? z.Select(x => new ExplorerItemNode.Filter(x.Key, x.Value)).ToList() : null);

        /// <summary>
        /// Gets the explorer information nodes.
        /// </summary>
        /// <param name="manager">The resource.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public virtual Task<List<ExplorerInfoNode>> GetExplorerInfoNodesAsync(ExplorerManager manager, ExplorerItemNode item)
            => throw new NotImplementedException();

        #endregion
    }
}