using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewLicenceProcessLineItem : PhoenixBasePage
{   
    string strProcessid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			SessionUtil.PageAccessRights(this.ViewState);
            strProcessid = Request.QueryString["pid"];
            if (!IsPostBack)
            {               
                PhoenixToolbar toolbar = new PhoenixToolbar();                
                toolbar.AddButton("Save", "SAVE");
				CrewMenu.AccessRights = this.ViewState;
                CrewMenu.MenuList = toolbar.Show();               
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                string strReqid = string.Empty;
                if (gvMissingLicence.Rows.Count > 1 || (gvMissingLicence.Rows.Count == 1 && gvMissingLicence.Rows[0].Cells.Count > 1))
                {
                    foreach (GridViewRow gr in gvMissingLicence.Rows)
                    {
                        CheckBox chk = (CheckBox)gr.FindControl("chk_select");
                        if (chk.Checked)
                            strReqid += ((Label)gr.FindControl("lblReqId")).Text + ",";
                    }
                    strReqid = strReqid.TrimEnd(',');
                }
                if (string.IsNullOrEmpty(strReqid))
                {
                    ucError.ErrorMessage = "Atleast select one licence request to add.";
                    ucError.Visible = true;
                    return;
                }                                
                PhoenixCrewLicenceRequest.UpdateCrewLicenceProcessLineItem(new Guid(strProcessid), strReqid);
                ucStatus.Text = "Licence Request Added.";
                string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('chml','ifMoreInfo');";
                Script += "</script>" + "\n";

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        try
        {
            DataTable dt = PhoenixCrewLicenceRequest.ListCrewLicenceProcessLineItem(new Guid(strProcessid));
            if (dt.Rows.Count > 0)
            {
                gvMissingLicence.DataSource = dt;
                gvMissingLicence.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvMissingLicence);
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
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
