#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void CreateAgent(AgentsView agentsView)
        {
            HispaniaCompData.Agent agentToCreate = agentsView.GetAgent();
            CreateAgentInDb(agentToCreate);
            agentsView.Agent_Id = agentToCreate.Agent_Id;
        }

        public void RefreshAgents()
        {
            try
            {
                AgentsInDb = HispaniaDataAccess.Instance.ReadAgents();
                _Agents = new ObservableCollection<AgentsView>();
                _AgentsInDictionary = new Dictionary<string, AgentsView>();
                _AgentsActiveInDictionary = new Dictionary<string, AgentsView>();
                foreach (HispaniaCompData.Agent agent in AgentsInDb)
                {
                    AgentsView NewAgentsView = new AgentsView(agent);
                    _Agents.Add(NewAgentsView);
                    _AgentsInDictionary.Add(GetKeyAgentView(agent), NewAgentsView);
                    if (!NewAgentsView.Canceled) _AgentsActiveInDictionary.Add(GetKeyAgentView(agent), NewAgentsView);
                }
            }
            catch (Exception ex)
            {
                _Agents = null;
                throw ex;
            }
        }

        private ObservableCollection<AgentsView> _Agents = null;

        public ObservableCollection<AgentsView> Agents
        {
            get
            {
                return _Agents;
            }
        }

        private Dictionary<string, AgentsView> _AgentsActiveInDictionary = null;

        public Dictionary<string, AgentsView> AgentsActiveDict
        {
            get
            {
                return _AgentsActiveInDictionary;
            }
        }

        private Dictionary<string, AgentsView> _AgentsInDictionary = null;

        public Dictionary<string, AgentsView> AgentsDict
        {
            get
            {
                return _AgentsInDictionary;
            }
        }

        public string GetKeyAgentView(AgentsView agentsView)
        {
            return GetKeyAgentView(agentsView.Agent_Id, agentsView.Name);
        }

        private string GetKeyAgentView(HispaniaCompData.Agent agent)
        {
            return GetKeyAgentView(agent.Agent_Id, agent.Name);
        }

        private string GetKeyAgentView(int Agent_Id, string Agent_Name)
        {
            return string.Format("{0} | {1}", Agent_Id, Agent_Name);
        }

        public void UpdateAgent(AgentsView AgentsView)
        {
            UpdateAgentInDb(AgentsView.GetAgent());
        }
        public void DeleteAgent(AgentsView AgentsView)
        {
            DeleteAgentInDb(AgentsView.GetAgent());
        }

        public AgentsView GetAgentFromDb(AgentsView agentsView)
        {
            return new AgentsView(GetAgentInDb(agentsView.Agent_Id));
        }

        public HispaniaCompData.Agent GetAgent(int Agents_Id)
        {
            return GetAgentInDb(Agents_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateAgentInDb(HispaniaCompData.Agent agent)
        {
            HispaniaDataAccess.Instance.CreateAgent(agent);
        }

        private List<HispaniaCompData.Agent> _AgentsInDb;

        private List<HispaniaCompData.Agent> AgentsInDb
        {
            get
            {
                return (this._AgentsInDb);
            }
            set
            {
                this._AgentsInDb = value;
            }
        }

        private void UpdateAgentInDb(HispaniaCompData.Agent agent)
        {
            HispaniaDataAccess.Instance.UpdateAgent(agent);
        }

        private void DeleteAgentInDb(HispaniaCompData.Agent agent)
        {
            HispaniaDataAccess.Instance.DeleteAgent(agent);
        }

        private HispaniaCompData.Agent GetAgentInDb(int Agents_Id)
        {
            return HispaniaDataAccess.Instance.GetAgent(Agents_Id);
        }

        #endregion
    }
}
