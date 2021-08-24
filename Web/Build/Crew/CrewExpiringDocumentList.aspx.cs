﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewExpiringDocumentList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvExpiringLicence.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        foreach (GridViewRow r in GvCourse.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        foreach (GridViewRow r in GvOtherDocs.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        foreach (GridViewRow r in GvTravelDocs.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        foreach (GridViewRow r in GvMedical.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }


        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {                
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Expiring Documents", "EXPIRINGDOCUMENTS");
                toolbarmain.AddButton("Missing Documents", "MISSINGDOCUMENTS");
                MenuCrewExpiringDocList.AccessRights = this.ViewState;
                MenuCrewExpiringDocList.MenuList = toolbarmain.Show();                
                MenuCrewExpiringDocList.SelectedMenuIndex = 0;

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../Crew/CrewExpiringDocumentList.aspx", "Export to Excel", "icon_xls.png", "Excel");
                MenuCrewExpiringDocs.AccessRights = this.ViewState;
                MenuCrewExpiringDocs.MenuList = toolbar.Show();

                if (Request.QueryString["empid"] != null)
                {
                    ViewState["empid"] = Request.QueryString["empid"].ToString();
                    SetEmployeePrimaryDetails();
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            DataSet ds = PhoenixCrewManagement.CrewExpiringDocumentsList(
                                                General.GetNullableInteger(ViewState["empid"].ToString())
                                                );

            BindLicenceData(ds.Tables[0]);
            BindCourseData(ds.Tables[1]);
            BindOtherDocumentData(ds.Tables[2]);
            BindTravelDocumentData(ds.Tables[3]);
            BindMedicalData(ds.Tables[4]);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindLicenceData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            gvExpiringLicence.DataSource = dt;
            gvExpiringLicence.DataBind();
        }
        else
            ShowNoRecordsFound(dt, gvExpiringLicence);
    }
    private void BindCourseData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            GvCourse.DataSource = dt;
            GvCourse.DataBind();
        }
        else
            ShowNoRecordsFound(dt, GvCourse);
    }
    private void BindOtherDocumentData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {

            GvOtherDocs.DataSource = dt;
            GvOtherDocs.DataBind();
        }
        else
            ShowNoRecordsFound(dt, GvOtherDocs);
    }
    private void BindTravelDocumentData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            GvTravelDocs.DataSource = dt;
            GvTravelDocs.DataBind();
        }
        else
            ShowNoRecordsFound(dt, GvTravelDocs);
    }
    private void BindMedicalData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            GvMedical.DataSource = dt;
            GvMedical.DataBind();
        }
        else
            ShowNoRecordsFound(dt, GvMedical);
    }
    protected void MenuCrewExpiringDocList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXPIRINGDOCUMENTS"))
            {
                Response.Redirect("../Crew/CrewExpiringDocumentList.aspx?empid=" + int.Parse(ViewState["empid"].ToString()));
            }
            else if (dce.CommandName.ToUpper().Equals("MISSINGDOCUMENTS"))
            {
                Response.Redirect("../Crew/CrewMissingDocumentList.aspx?empid=" + int.Parse(ViewState["empid"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewExpiringDocs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDDOCUMENTNAME", "FLDDATEOFEXPIRY", "FLDFLAGNAME" };
                string[] alCaptions = { "Document Name", "Expiry Date", "Nationality" };

                DataSet ds = PhoenixCrewManagement.CrewExpiringDocumentsList(
                                               General.GetNullableInteger(ViewState["empid"].ToString())
                                               );
                
                Response.AddHeader("Content-Disposition", "attachment; filename=CrewExpiringDocumentList.xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr>");
                Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
                Response.Write("<td><h3> Expiring Document List</h3></td>");
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
                for (int d=0;d<ds.Tables.Count;d++)
                {
                    DataTable dt = ds.Tables[d];
                    if (dt.Rows.Count > 0)
                    {
                        Response.Write("<tr>");
                        Response.Write("<td colspan='3'>");
                        Response.Write("<b>" + dt.Rows[0]["FLDDOCFOR"] + "</b>");
                        Response.Write("</td>");

                        foreach (DataRow dr in dt.Rows)
                        {
                            Response.Write("<tr>");
                            for (int i = 0; i < alColumns.Length; i++)
                            {
                                Response.Write("<td align='left'>");
                                Response.Write(dr[alColumns[i]]);
                                Response.Write("</td>");
                            }
                            Response.Write("</tr>");
                        }                       
                    }
                }

                Response.Write("</TABLE>");
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Request.QueryString["empid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
}
