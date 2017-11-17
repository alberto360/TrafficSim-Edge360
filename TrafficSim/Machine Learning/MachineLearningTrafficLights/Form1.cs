using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineLearningTrafficLights
{
    public partial class FormLearnLights : Form
    {
        class SimulationNode
        {
            public Dictionary<Guid, float> LightDictionary = null;

            //Only store the simulations that are better than I am
            List<SimulationNode> SubSims = new List<SimulationNode>();

            public float MyScore;

            private FormLearnLights parentForm = null;

            public SimulationNode(Dictionary<Guid, float> NodeList, FormLearnLights parent)
            {
                parentForm = parent;
                //spawn sub simulations
                LightDictionary = NodeList;

                float returnedScore = 0; //Insert function I'm calling in the simulation into here
            }

            public void showResults()
            {
                parentForm.DisplayText(Environment.NewLine + Environment.NewLine + "Ideal light results:" + Environment.NewLine);

                foreach (var light in LightDictionary)
                {
                    parentForm.DisplayText(light.Key + ":" + light.Value);
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="NodeList"></param>
            /// <returns>Best node found in the simulation</returns>
            public float runSim()
            {
                //Call Alberto's function

                MyScore = 0;

                return 0;
            }
        }

        public FormLearnLights()
        {
            InitializeComponent();
        }

        public void DisplayText(string Text)
        {
            textBoxShowResults.Text += Text;
        }

        private void btnNewSim_Click(object sender, EventArgs e)
        {
            float diffAmount = 5;

            //Insert initial values into dictionary
            Dictionary<Guid, float> initialValues = new Dictionary<Guid, float>();   //Dictionary<Guid, float> GetDefaultValues();  <- plug in Alberto's function once ready

            SimulationNode parentNode = new SimulationNode(initialValues, this);

            SimulationNode curNode = FindBestSimulationNode(parentNode, 3);
            curNode.showResults();

            curNode = FindBestSimulationNode(curNode, 1);
            curNode.showResults();

        }

        private SimulationNode FindBestSimulationNode(SimulationNode initialNode, float diffAmount)
        {
            int prevScore = 0;

            SimulationNode curNode = initialNode;

            float curScore = initialNode.runSim();

            while (curScore > prevScore + 0.1)
            {
                SimulationNode curBest = curNode;

                object lockOnPickingBest = new object();

                Parallel.ForEach(curNode.LightDictionary, light =>
                {
                    //Create new Simulation Node
                    SimulationNode simMeUp = new SimulationNode(curNode.LightDictionary, this);
                    SimulationNode simMeDown = new SimulationNode(curNode.LightDictionary, this);

                    //New Simulation Node will be the same as the last one BUT light will be tweaked up.. then down
                    simMeUp.LightDictionary[light.Key] += diffAmount;
                    simMeDown.LightDictionary[light.Key] -= diffAmount;

                    if (simMeUp.LightDictionary[light.Key] > 300)
                    {
                        simMeUp.LightDictionary[light.Key] = 300;
                    }

                    if (simMeUp.LightDictionary[light.Key] < 5)
                    {
                        simMeUp.LightDictionary[light.Key] = 5;
                    }

                    //Run it's simulation
                    float scoreUp = simMeUp.runSim();
                    float scoreDown = simMeDown.runSim();

                    //Pick the best one, and repeat
                    lock (lockOnPickingBest)
                    {
                        if (scoreUp > curBest.MyScore)
                        {
                            curBest = simMeUp;
                        }

                        if (scoreDown > curBest.MyScore)
                        {
                            curBest = simMeDown;
                        }
                    }


                });
            }
            return initialNode;
        }

        private void btnNewSimulation_Click(object sender, EventArgs e)
        {

        }
    }
}
