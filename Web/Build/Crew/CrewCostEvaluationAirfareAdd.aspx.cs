using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewCostEvaluationAirfareAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Search", "SEARCH");
            toolbarmain.AddButton("Save", "ADD");

            MenuCrewAirfare.AccessRights = this.ViewState;
            MenuCrewAirfare.MenuList = toolbarmain.Show();

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FROMCITYID"] = null;
                ViewState["REQUESTID"] = null;
                ViewState["EVALUATIONPORTID"] = null;

                if (Request.QueryString["requestid"] != null)
                {
                    ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
                    if (Request.QueryString["evaluationportid"] != null)
                    {
                        if (Request.QueryString["evaluationportid"].ToString() != "")
                        {
                            ViewState["EVALUATIONPORTID"] = Request.QueryString["evaluationportid"].ToString();
                            DataTable dt = new DataTable();
                            dt = PhoenixCrewCostEvaluationRequest.EditCrewCostEvaluationPort(new Guid(ViewState["REQUESTID"].ToString()), General.GetNullableGuid(ViewState["EVALUATIONPORTID"].ToString()));
                            ViewState["FROMCITYID"] = dt.Rows[0]["FLDCITYID"].ToString();
                        }
                    }
                }

            }
            BindData();
            SetPageNavigator();
            if (General.GetNullableInteger(ViewState["FROMCITYID"].ToString()) == null)
            {
                ucError.ErrorMessage = "The selected port not have city";
                ucError.Visible = true;
                return;
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;

            DataSet ds = new DataSet();

            ds = PhoenixCrewCostEvaluationRequest.CrewCostStandardAirfareSearch(
                General.GetNullableInteger(ViewState["FROMCITYID"].ToString())
                , General.GetNullableInteger(ddlCity.SelectedCity)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"]
                , 10
                , ref iRowCount
                , ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewAirfare.DataSource = ds;
                gvCrewAirfare.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewAirfare);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvCrewAirfare_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["SORTEXPRESSION"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                            img.Src = Session["images"] + "/arrowUp.png";
                        else
                            img.Src = Session["images"] + "/arrowDown.png";

                        img.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewAirfare_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper() == "SEARCH")
            {
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper() == "ADD")
            {
                StringBuilder strAirfareIdList = new StringBuilder();

                foreach (GridViewRow gvr in gvCrewAirfare.Rows)
                {
                    CheckBox chkCheck = (CheckBox)gvr.FindControl("chkCheck");

                    if (chkCheck != null)
                    {
                        if (chkCheck.Checked && chkCheck.Enabled == true)
                        {
                            Label lblCrewAirfareId = (Label)gvr.FindControl("lblCrewAirfareId");

                            strAirfareIdList.Append(lblCrewAirfareId.Text);
                            strAirfareIdList.Append(",");
                        }
                    }
                }
                if (strAirfareIdList.Length > 1)
                {
                    strAirfareIdList.Remove(strAirfareIdList.Length - 1, 1);
                }
                if (!IsValidAirfare(strAirfareIdList.ToString(), (ViewState["FROMCITYID"] == null ? "" : ViewState["FROMCITYID"].ToString())))
                {
                    ucError.Visible = true;
                    BindData();
                    return;
                }
                //else
                //{
                //    if (ViewState["REQUESTID"] != null)
                //    {                         
                //            PhoenixCrewCostEvaluationRequest.InsertCrewCostEvaluationAirfare(General.GetNullableGuid(ViewState["REQUESTID"] != null ? ViewState["REQUESTID"].ToString() : "")
                //                , General.GetNullableGuid(ViewState["EVALUATIONPORTID"] != null ? ViewState["EVALUATIONPORTID"].ToString() : "")
                //                , strAirfareIdList.ToString());                      

                //    }
                //    string Script = "";
                //    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //    Script += " fnReloadList();";
                //    Script += "</script>" + "\n";
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
                //}
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvCrewAirfare.EditIndex = -1;
        gvCrewAirfare.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCrewAirfare.SelectedIndex = -1;
        gvCrewAirfare.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }
    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }
    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }
    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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
        gv.Rows[0].Attributes["onclick"] = "";
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvCrewAirfare.EditIndex = -1;
        gvCrewAirfare.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }
    private bool IsValidAirfare(string strAirfareIdList,string cityid)
    {
        if (General.GetNullableInteger(cityid) == null)
        {
            ucError.ErrorMessage = "The selected port does not have city";
        }
        else if (strAirfareIdList.Trim() == "")
        {
            ucError.ErrorMessage = "Please select atleast one airfare";
        }
        return (!ucError.IsError);
    }
    protected void ddlCountry_TextChanged(object sender, EventArgs e)
    {
        ddlState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlCountry.SelectedCountry));
    }
    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ddlCountry.SelectedCountry), General.GetNullableInteger(ddlCity.SelectedCity));
    }
    protected void ddlCity_TextChanged(object sender, EventArgs e)
    {
        BindData();
    }

}
