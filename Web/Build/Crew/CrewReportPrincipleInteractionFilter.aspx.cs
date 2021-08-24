using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using Telerik.Web.UI;
public partial class Crew_CrewReportPrincipleInteractionFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO");
        toolbar.AddButton("Cancel", "CANCEL");
        PrincipleInteractionFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            btnSubmit.Attributes.Add("style", "border:0px;background-color:White");
           
            ucPrincipal.Focus();
            cblVessel.DataSource = PhoenixRegistersVessel.ListVessel(null, "", 1);
            cblVessel.DataTextField = "FLDVESSELNAME";
            cblVessel.DataValueField = "FLDVESSELID";
            
            cblVessel.DataBind();
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();
        StringBuilder strvessel = new StringBuilder();
        criteria.Clear();
        foreach (ListItem item in cblVessel.Items)
        {
            if (item.Selected == true)
            {
                strvessel.Append(item.Value.ToString());
                strvessel.Append(",");
            }
        }

        if (strvessel.Length > 1)
        {
            strvessel.Remove(strvessel.Length - 1, 1);
        }
        criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
        criteria.Add("cblVessel", strvessel.ToString());
        
        Filter.CurrentNewApplicantFilterCriteria = criteria;

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript1", Script);
    }
    protected void PrincipleInteractionFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            StringBuilder strvessel = new StringBuilder();
            criteria.Clear();
            string vessel;
            foreach (ListItem item in cblVessel.Items)
            {
                if (item.Selected == true)
                {
                    strvessel.Append(item.Value.ToString());
                    strvessel.Append(",");
                }
            }

            if (strvessel.Length > 1)
            {
                strvessel.Remove(strvessel.Length - 1, 1);
            }
            vessel = strvessel.ToString();
            if (strvessel.ToString() == "")
            {
                vessel = null;
            }

            criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
            criteria.Add("cblVessel", vessel);


            Filter.CurrentPrincipleInteractionFilterCriteria = criteria;
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
