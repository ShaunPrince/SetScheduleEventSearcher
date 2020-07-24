using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using JsonTest;

namespace CustomSearchWindowsForm
{
    public partial class EventSearchForm : Form
    {

        // delaying variables to attempt to reduce crashes due to overided memory access.
        private bool isAutoCompUpdating = false;
        private bool firstAutoEntry = true;
        private DateTime timeOfEntry;
        private bool currentlyDelayingFurtherEntry = false;

        private Stack<string> autocompStack = new Stack<string>();

        public EventSearchForm()
        {
            InitializeComponent();
        }

        private void eventTextbox_TextChanged(object sender, EventArgs e)
        {
            //Delay the entry of further data to attempt to avoid crashes.

            locationTextbox.Visible = false;
            radiusTextbox.Visible = false;
            timeOfEntry = System.DateTime.Now;
            if(currentlyDelayingFurtherEntry == false)
            {
                currentlyDelayingFurtherEntry = true;
                while (System.DateTime.Now - timeOfEntry < TimeSpan.FromSeconds(.5f))
                {
                    //wait to allow future data entry
                }
                locationTextbox.Visible = true;
                radiusTextbox.Visible = true;
                currentlyDelayingFurtherEntry = false;

            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (eventTextbox.Text != "" &&
                locationTextbox.Text != "" &&
                radiusTextbox.Text != "" &&
                float.TryParse(radiusTextbox.Text, out float result) && 
                result > 0 )
            {
                HideAllWarnings();

                //get lat long of selected place
                LatLongFromLocation latLongLocation = new LatLongFromLocation();
                Tuple<float,float> latLong = latLongLocation.GetLatLong(locationTextbox.Text);

                string eventSearchUrl = EventSearchUrlBuilder.BuildSearchUrl(eventTextbox.Text, latLong.Item1.ToString(), latLong.Item2.ToString(), radiusTextbox.Text);
                string results = HTTPrequests.GetHTTPRequestWithTicketMaster(eventSearchUrl);

                AllEventsTopItem items = EventHtmlToJsonConverter.ConvertRawHtmlToJson(results);

                if (float.Equals(latLong.Item1, -404) || float.Equals(latLong.Item2, -404))
                {
                    DisplayWarning("failed");
                }
                else
                {
                    // dynamically populate all the events on the right hand side

                    ClearSidePanel();
                    if(items._embedded == null)
                    {
                        DisplayWarning("noResults");

                    }
                    else
                    {
                        for (int i = 0; i < items._embedded.events.Count; i++)
                        {
                            CreateSidePanelEntry(items._embedded.events[i].name, items._embedded.events[i].info,
                                items._embedded.events[i].dates.start.localDate + " at " + items._embedded.events[i].dates.start.localTime,
                                items._embedded.events[i].url, i);
                        }
                    }

                }
            }
            else
            {
                DisplayWarning("warning");
            }
        }



        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void locationTextbox_TextChanged(object sender, EventArgs e)
        {
            autocompStack.Push(locationTextbox.Text);
            //doesAutoCompNeedToUpdate = true;
            if (firstAutoEntry)
            {
                firstAutoEntry = false;
                isAutoCompUpdating = true;
                UpdateAutoComplete();
            }
            else if (!isAutoCompUpdating)
            {
                isAutoCompUpdating = true;
                UpdateAutoComplete();
            }
        }

        private void UpdateAutoComplete()
        {
            //isAutoCompUpdating = true;

            while (autocompStack.Count > 0)
            {
                string text = autocompStack.Pop();
                autocompStack.Clear();
                var autoCom = new AutoCompleteStringCollection();
                AutocompleteLocation autocomplete = new AutocompleteLocation();
                string[] items = autocomplete.GetAutocomplete(text);
                autoCom.AddRange(items);
                locationTextbox.AutoCompleteCustomSource = autoCom;
            }

            isAutoCompUpdating = false;
        }

        private void CreateSidePanelEntry(string name, string info, string dateTimeStr, string linkURL , int numElement)
        {
            int yDisplacement = 69 * numElement;
            int topPadding = 10;

            Label labelName = new Label();
            labelName.Location = new Point(3, yDisplacement);
            labelName.Padding = new Padding(0, topPadding *2, 0, 0);
            labelName.AutoSize = true;
            labelName.Text = name;
            labelName.Font = new System.Drawing.Font(labelName.Font , System.Drawing.FontStyle.Bold);

            displayPanel.Controls.Add(labelName);

            if (info !=  null)
            {
                yDisplacement += 23;

                Label labelInfo = new Label();
                labelInfo.Location = new Point(3, yDisplacement);
                labelInfo.Padding = new Padding(0, topPadding, 0, 0);
                labelInfo.MaximumSize = new Size(400, 0);
                labelInfo.AutoSize = true;
                labelInfo.Text = info;

                displayPanel.Controls.Add(labelInfo);
            }

            yDisplacement += 23;

            Label dateTime = new Label();
            dateTime.Location = new Point(3, yDisplacement);
            dateTime.Padding = new Padding(0, topPadding, 0, 0);
            //linkLabel.Padding = new Padding(0, topPadding, 0,0);
            dateTime.AutoSize = true;
            dateTime.Text = dateTimeStr;

            displayPanel.Controls.Add(dateTime);


            yDisplacement += 23;

            LinkLabel linkLabel = new LinkLabel();
            linkLabel.Location = new Point(3, yDisplacement);
            linkLabel.Padding = new Padding(0, topPadding, 0, 0);
            //linkLabel.Padding = new Padding(0, topPadding, 0,0);
            linkLabel.AutoSize = true;
            linkLabel.Text = linkURL;
            linkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(LinkedLabelClicked);

            displayPanel.Controls.Add(linkLabel);
        }

        private void LinkedLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Console.WriteLine(sender.GetType());
            Console.WriteLine(e);
            string goToUrl = ((LinkLabel)sender).Text;
            ProcessStartInfo sInfo = new ProcessStartInfo(goToUrl);
            Process.Start(sInfo);
        }

        private void ClearSidePanel()
        {
            displayPanel.Controls.Clear();
        }

        private void DisplayWarning(string type)
        {
            if (type.ToLower() == "warning")
            {
                warningLabel.Visible = true;
            }
            else if (type.ToLower() == "failed")
            {
                failLabel.Visible = true;
            }
            else if (type.ToLower() == "noresults")
            {
                EmptyResultsWarning.Visible = true;
            }
        }

        private void HideAllWarnings()
        {
            warningLabel.Visible = false;
            failLabel.Visible = false;
            EmptyResultsWarning.Visible = false;
        }

        private void radiusTextbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
