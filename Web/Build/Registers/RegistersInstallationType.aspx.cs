using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Registers_RegistersInstallationType : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersInstallationType.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvInstallation')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersInstallationType.aspx", "Find", "search.png", "FIND");
            // toolbar.AddFontAwesomeButton("../Registers/RegistersInstallationType.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuRegistersInstallation.AccessRights = this.ViewState;
            MenuRegistersInstallation.MenuList = toolbar.Show();
            //  MenuRegistersInstallation.SetTrigger(pnlInstallationEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                BindData();
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDTYPEOFINSTALLATIONCODE", "FLDTYPEOFINSTALLATIONNAME" };
        string[] alCaptions = { "Installation Code", "Installation Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixRegistersTypeOfInstallation.TypeOfInstallationSearch(txtSearch.Text, txtInstallation.Text, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MaritalStatus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Installation Type</h3></td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void RegistersInstallation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvInstallation.Rebind();

            }
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

        string[] alColumns = { "FLDTYPEOFINSTALLATIONCODE", "FLDTYPEOFINSTALLATIONNAME" };
        string[] alCaptions = { "Installation Code", "Installation Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersTypeOfInstallation.TypeOfInstallationSearch(txtSearch.Text, txtInstallation.Text, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvInstallation", "Installation Type", alCaptions, alColumns, ds);


        gvInstallation.DataSource = ds;
        gvInstallation.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    
    private void InsertInstallation(string Installationname, string Installationcode)
    {
        if (!IsValidInstallation(Installationname, Installationcode))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersTypeOfInstallation.InsertTypeOfInstallation(Installationname, Installationcode);
    }

    private void UpdateInstallation(int Installationid, string Installationcode, string Installationname)
    {
        PhoenixRegistersTypeOfInstallation.UpdateTypeOfInstallation(Installationid, Installationcode, Installationname);
        ucStatus.Text = "Installation information updated";
    }

    private bool IsValidInstallation(string Installationname, string Installationcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (Installationcode.Trim().Equals(""))
            ucError.ErrorMessage = "Installation Code is required.";

        if (Installationname.Trim().Equals(""))
            ucError.ErrorMessage = "Installation Name is required.";


        return (!ucError.IsError);
    }

    //private void DeleteInstallation(int InstallationID)
    //{
    //    PhoenixRegistersTypeOfInstallation.DeleteTypeOfInstallation(InstallationID);
    //}



    protected void gvInstallation_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                int InstallationID = Int32.Parse(((RadLabel)e.Item.FindControl("lblInstallationID")).Text);
                PhoenixRegistersTypeOfInstallation.DeleteTypeOfInstallation(InstallationID);
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidInstallation(((RadTextBox)e.Item.FindControl("txtInstallationEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtInstallationCodeEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateInstallation(
                         Int32.Parse(((RadLabel)e.Item.FindControl("lblInstallationIDEdit")).Text),
                         ((RadTextBox)e.Item.FindControl("txtInstallationCodeEdit")).Text,
                         ((RadTextBox)e.Item.FindControl("txtInstallationEdit")).Text
                     );
                gvInstallation.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidInstallation(((RadTextBox)e.Item.FindControl("txtInstallationAdd")).Text,
                 ((RadTextBox)e.Item.FindControl("txtInstallationCodeAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertInstallation(
                    ((RadTextBox)e.Item.FindControl("txtInstallationAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtInstallationCodeAdd")).Text);
                gvInstallation.Rebind();



            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvInstallation.SelectedIndexes.Clear();
        gvInstallation.EditIndexes.Clear();
        gvInstallation.DataSource = null;
        gvInstallation.Rebind();
    }

    protected void gvInstallation_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }

    }

    protected void gvInstallation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInstallation.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
