using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class CrewTravelLocalArrangements : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                
                cblArrangements.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 216);
                cblArrangements.DataBind();
                BindField();
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE");
            MenuTravel.AccessRights = this.ViewState;
            MenuTravel.MenuList = toolbarmain.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindField()
    {
        if ((Request.QueryString["HOPLINEITEMID"] != null) && (Request.QueryString["HOPLINEITEMID"] != ""))
        {
            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelLocalArrangementsList(new Guid(Request.QueryString["HOPLINEITEMID"]));
            DataRow dr = ds.Tables[0].Rows[0];
            string str = ds.Tables[0].Rows[0]["FLDARRANGEMENTS"].ToString();
            if (str.Length > 1)
            {
                string[] Arrangements = str.Split(',');
                foreach (string s in Arrangements)
                {
                    foreach (ListItem item in cblArrangements.Items)
                    {
                        if (int.Parse(s) == int.Parse(item.Value))
                            item.Selected = true;
                    }
                }
            }

        }
    }
    protected void MenuTravel_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper() == "SAVE")
            {
                if ((Request.QueryString["HOPLINEITEMID"] != null) && (Request.QueryString["HOPLINEITEMID"] != ""))
                {
                    StringBuilder strArrangementsList = new StringBuilder();

                    foreach (ListItem item in cblArrangements.Items)
                    {
                        if (item.Selected == true)
                        {
                            strArrangementsList.Append(item.Value);
                            strArrangementsList.Append(",");
                        }
                    }
                    if (strArrangementsList.Length > 1)
                    {
                        strArrangementsList.Remove(strArrangementsList.Length - 1, 1);
                    }
                    PhoenixCrewTravelQuoteLine.CrewTravelLocalArrangementsUpadte(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Request.QueryString["HOPLINEITEMID"]), General.GetNullableString(strArrangementsList.ToString()));
                }
                BindField();
                ucStatus.Text = "Information Saved";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
