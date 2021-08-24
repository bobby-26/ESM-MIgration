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
public partial class CrewVesselArrivalPortList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Add ", "ADD", ToolBarDirection.Right);
            toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);            
            MenuArrivalPort.AccessRights = this.ViewState;
            MenuArrivalPort.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["VESSELID"] = "";
                ViewState["REQUESTID"] = "";

                if (Request.QueryString["REQUESTID"] != null)
                {
                    ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
                }
                if (Request.QueryString["VESSELID"] != null)
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                    DataSet ds = new DataSet();
                    ds = PhoenixRegistersVessel.EditVessel(int.Parse(ViewState["VESSELID"].ToString()));
                    if (ds.Tables[0].Rows.Count > 0)
                        txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                }
                txtDate.Text = DateTime.Today.ToString();

                gvVesselPort.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
            DataTable dt = new DataTable();

            if (General.GetNullableDateTime(txtDate.Text) == null)
            {
                ucError.Visible = true;
                ucError.Text = "Date Required";
            }
            else
            {
                dt = PhoenixCrewCostEvaluationRequest.ListCrewVesselArrivalPort(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                     , General.GetNullableDateTime(txtDate.Text)
                     , General.GetNullableGuid(ViewState["REQUESTID"].ToString())
                     );
            }

            gvVesselPort.DataSource = dt;
        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuArrivalPort_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "SEARCH")
            {
                BindData();
            }
            if (CommandName.ToUpper() == "ADD")
            {
                StringBuilder strPortList = new StringBuilder();

                foreach (GridDataItem gvr in gvVesselPort.Items)
                {
                    RadCheckBox chkCheck = (RadCheckBox)gvr.FindControl("chkCheck");

                    if (chkCheck != null)
                    {
                        if (chkCheck.Checked== true && chkCheck.Enabled == true)
                        {
                            RadLabel lblPortCallId = (RadLabel)gvr.FindControl("lblPortCallId");

                            strPortList.Append(lblPortCallId.Text);
                            strPortList.Append(",");
                        }
                    }
                }
                if (!IsValidPort(strPortList.ToString()))
                {
                    ucError.Visible = true;
                    BindData();
                    return;
                }
                else
                {
                    foreach (GridDataItem gvr in gvVesselPort.Items)
                    {
                        RadCheckBox chkCheck = (RadCheckBox)gvr.FindControl("chkCheck");

                        if (chkCheck != null)
                        {
                            if (chkCheck.Checked == true && chkCheck.Enabled == true)
                            {
                                RadLabel lblPortCallId = (RadLabel)gvr.FindControl("lblPortCallId");

                                PhoenixCrewCostEvaluationRequest.InsertCrewCostEvaluationPort(General.GetNullableGuid(ViewState["REQUESTID"].ToString())
                                   , General.GetNullableGuid(lblPortCallId.Text));
                            }
                        }
                    }
                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " fnReloadList();";
                    Script += "</script>" + "\n";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private bool IsValidPort(string portlist)
    {
        if (portlist.Trim() == "")
        {
            ucError.ErrorMessage = "Please Select Atleast One Port";
        }
        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void gvVesselPort_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvVesselPort_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvVesselPort_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
}
