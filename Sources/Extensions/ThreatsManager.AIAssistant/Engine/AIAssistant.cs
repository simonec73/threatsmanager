using Azure;
using Azure.AI.OpenAI;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.AIAssistant.Engine
{
    /// <summary>
    /// AI Assistant engine.
    /// </summary>
    /// <remarks>This is the main entry point to use the OpenAI services for threat modeling.</remarks>
    public class AIAssistant
    {
        #region Private member variables.
        private readonly OpenAIClient _client;
        private readonly string _deploymentName;
        #endregion

        #region Constructors.
        /// <summary>
        /// Constructor to access OpenAI APIs.
        /// </summary>
        /// <param name="key">API key to use.</param>
        /// <param name="engine">OpenAI ChatGPT client. If missing, ChatGPT 3.5 will be used.</param>
        /// <remarks>It uses the default version of the APIs.</remarks>
        public AIAssistant([Required] string key, string engine = "gpt-3.5-turbo")
        {
            _client = new OpenAIClient(key);
            _deploymentName = engine;
        }

        /// <summary>
        /// Constructor to access OpenAI APIs.
        /// </summary>
        /// <param name="key">API key to use.</param>
        /// <param name="options">Options for the client.</param>
        /// <param name="engine">OpenAI ChatGPT client. If missing, ChatGPT 3.5 will be used.</param>
        public AIAssistant([Required] string key, [NotNull] OpenAIClientOptions options, 
            string engine = "gpt-3.5-turbo")
        {
            _client = new OpenAIClient(key, options);
            _deploymentName = engine;
        }

        /// <summary>
        /// Constructor to access Azure OpenAI APIs.
        /// </summary>
        /// <param name="url">URL of the APIs.</param>
        /// <param name="deploymentName">Name of the deployment to be used.</param>
        /// <param name="key">API key to use.</param>
        public AIAssistant([Required] string url, [Required] string deploymentName, [Required] string key)
        {
            _client = new OpenAIClient(new Uri(url), new AzureKeyCredential(key));
            _deploymentName = deploymentName;
        }

        /// <summary>
        /// Constructor to access Azure OpenAI APIs.
        /// </summary>
        /// <param name="url">URL of the APIs.</param>
        /// <param name="deploymentName">Name of the deployment to be used.</param>
        /// <param name="key">API key to use.</param>
        /// <param name="options">Options for the client.</param>
        public AIAssistant([Required] string url, [Required] string deploymentName, 
            [Required] string key, [NotNull] OpenAIClientOptions options)
        {
            _client = new OpenAIClient(new Uri(url), new AzureKeyCredential(key), options);
            _deploymentName = deploymentName;
        }
        #endregion

        #region Public member functions.
        /// <summary>
        /// Get a list of questions related to the Entity passed as argument.
        /// </summary>
        /// <param name="entity">Entity to be analyzed.</param>
        /// <returns></returns>
        public IEnumerable<string> GetQuestions([NotNull] IEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AIThreat> GetThreats([NotNull] IEntity entity)
        {  throw new NotImplementedException(); }

        public IEnumerable<AIMitigation> GetMitigations([NotNull] IEntity entity, [NotNull] AIThreat threat)
        { throw new NotImplementedException(); }
        #endregion

        #region Private member functions.
        private string GetThreatModelDescription([NotNull] IThreatModel model)
        {
            throw new NotImplementedException();
        }
        private string GetEntityDescription([NotNull] IEntity entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
