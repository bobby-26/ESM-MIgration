using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
using SouthNests.Phoenix.PlannedMaintenance;
public partial class PlannedMaintenanceJobParameterOptions : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/PlannedMaintenanceJobParameterOptions.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFBQOptions')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersFBQOptions.AccessRights = this.ViewState;
            MenuRegistersFBQOptions.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["Parameterid"] = Request.QueryString["parameterid"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindEdit();
                gvJobParameterOptions.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindEdit()
    {
        DataTable dt = PhoenixPlannedMaintenanceJobParameterOptions.JobParameterEdit(new Guid(ViewState["Parameterid"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtParameterName.Text = dt.Rows[0]["FLDPARAMETERNAME"].ToString();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDLEVEL", "FLDOPTIONNAME" };
        string[] alCaptions = { "Order No.", "Option" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataTable dt = PhoenixPlannedMaintenanceJobParameterOptions.JobParameterOptionSearch(new Guid(ViewState["Parameterid"].ToString()), null
                                            , sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()), gvJobParameterOptions.PageSize
                                            , ref iRowCount, ref iTotalPageCount);
        ds = new DataSet();
        ds.Tables.Add(dt.Copy());


        Response.AddHeader("Content-Disposition", "attachment; filename=SignOffFeedBackQuestionOptions.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sign Off FeedBack Question Options</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(Regex.Replace(dr[alColumns[i]].ToString(), @"[^\u0000-\u007F]", string.Empty));
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuRegistersFBQOptions_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDLEVEL", "FLDOPTIONNAME" };
        string[] alCaptions = { "Order No.", "Option" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixPlannedMaintenanceJobParameterOptions.JobParameterOptionSearch(new Guid(ViewState["Parameterid"].ToString()), null
                                     , sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()), gvJobParameterOptions.PageSize
                                     , ref iRowCount, ref iTotalPageCount);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvJobParameter", "Job Parameter", alCaptions, alColumns, ds);

        gvJobParameterOptions.DataSource = dt;
        gvJobParameterOptions.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void Rebind()
    {
        gvJobParameterOptions.SelectedIndexes.Clear();
        gvJobParameterOptions.EditIndexes.Clear();
        gvJobParameterOptions.DataSource = null;
        gvJobParameterOptions.Rebind();
    }
    protected void gvJobParameterOptions_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() == "ADD")
            {
                UserControlMaskNumber Level = (UserControlMaskNumber)e.Item.FindControl("txtParameterLevel");
                RadTextBox Option = (RadTextBox)e.Item.FindControl("txtParameterOption");

                if (!IsValidOption(Level.Text, Option.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceJobParameterOptions.JobParameterOptionInsert(new Guid(ViewState["Parameterid"].ToString()), Option.Text, int.Parse(Level.Text));
                Rebind();
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string id = drv["FLDJOBPARAMETEROPTIONSID"].ToString();
                UserControlMaskNumber Level = (UserControlMaskNumber)e.Item.FindControl("txtParameterLevelEdit");
                RadTextBox Option = (RadTextBox)e.Item.FindControl("txtParameterOptionEdit");
                bool expectedans = ((RadCheckBox)e.Item.FindControl("chkExpectedAns")).Checked == true ? true : false;
                if (!IsValidOption(Level.Text, Option.Text))
                {
                    ucError.Visible = true;
                    e.Canceled = true;
                    return;
                }
                PhoenixPlannedMaintenanceJobParameterOptions.JobParameterOptionUpdate(new Guid(id), int.Parse(Level.Text), Option.Text, expectedans);
                Rebind();
            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string id = drv["FLDJOBPARAMETEROPTIONSID"].ToString();
                PhoenixPlannedMaintenanceJobParameterOptions.JobParameterOptionDelete(new Guid(id));
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvJobParameterOptions_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvJobParameterOptions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvJobParameterOptions.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    private bool IsValidOption(string OptionLevel, string OptionName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(OptionLevel) == null)
            ucError.ErrorMessage = "Order No. is required.";
        if (OptionName == string.Empty)
            ucError.ErrorMessage = "Option is required.";

        return (!ucError.IsError);
    }

}
