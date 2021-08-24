using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard_DashoboardInspectionVsDeficiency : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            #region constant
            const int C_STARTING_YEAR = 2011;
            #endregion
            PhoenixToolbar toolbarmain1 = new PhoenixToolbar();
            toolbarmain1.AddButton("PSC Deficiency", "PSCCHAPTERWISE");
            toolbarmain1.AddButton("PSC Inspection", "PSCINSPECCOUNT");
            MainMenu.AccessRights = this.ViewState;
            MainMenu.MenuList = toolbarmain1.Show();
            MainMenu.SelectedMenuIndex =1;
            toolbarmain1 = new PhoenixToolbar();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
             PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageLink("../Dashboard/DashoboardInspectionVsDeficiency.aspx", "Filter", "search.png", "FIND");
            MenuSearch.AccessRights = this.ViewState;
            MenuSearch.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                for (int i = C_STARTING_YEAR; i <= DateTime.Now.Year; i++)
                {
                    chkyear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
                DataSet ds = PhoenixRegistersAddress.ListPrincipal();
                DataTable dt = ds.Tables[0];
                foreach (DataRow row in dt.Rows)
                {

                    row.SetField("FLDNAME", textInfo.ToTitleCase(row["FLDNAME"].ToString().ToLower()));
                }
                rdbAddressPrincipal.DataSource = dt;
                rdbAddressPrincipal.DataTextField = "FLDNAME";
                rdbAddressPrincipal.DataValueField = "FLDADDRESSCODE";
                rdbAddressPrincipal.DataBind();

            }

            foreach (ListItem item in rdbAddressPrincipal.Items)
            {
                item.Attributes["title"] = item.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MainMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
           
             if (dce.CommandName.ToUpper().Equals("PSCCHAPTERWISE"))
            {
                Response.Redirect("../Dashboard/DashboardInspectionChapterWiseDeficiency.aspx", false);
            }

            else if (dce.CommandName.ToUpper().Equals("PSCINSPECCOUNT"))
            {
                Response.Redirect("../Dashboard/DashoboardInspectionVsDeficiency.aspx", false);
            }
              
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string YearSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in chkyear.Items)
        {
            if (item.Selected == true)
            {

                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }
        return strlist.ToString().TrimEnd(',');
    }
    protected void MenuSearch_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {

                DataSet ds = PhoenixInspectionChart.InspectionChart(rdbAddressPrincipal.SelectedValue, YearSelectedList(), "PSC");
                insDefData.Value = General.DataSetToJSONWithJSONNet(ds);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
