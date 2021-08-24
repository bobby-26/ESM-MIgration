using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using System.Collections;
using SouthNests.Phoenix.Common;

public partial class InspectionAuditInterfaceBulkUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuUpload.AccessRights = this.ViewState;
            MenuUpload.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["REVIEWSCHEDULEID"] != null)
                {
                    ViewState["REVIEWSCHEDULEID"] = Request.QueryString["REVIEWSCHEDULEID"].ToString();
                }

                if (Request.QueryString["QUESTIONTYPE"] != null)
                {
                    ViewState["QUESTIONTYPE"] = Request.QueryString["QUESTIONTYPE"].ToString();

                    BindQuestionType();
                }

            }            
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindQuestionType()
    {

        DataTable ds = new DataTable();
        btnAnsSearch.DataSource = PhoenixInspectionAuditInterfaceDetails.InspectionCheckitemAnswer
                                        (General.GetNullableInteger(ViewState["QUESTIONTYPE"].ToString()));
        btnAnsSearch.DataBind();
    }


    protected void MenuUpload_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int? No = General.GetNullableInteger(PhoenixCommonRegisters.GetQuickCode(188, "NO"));
                int? Poor = General.GetNullableInteger(PhoenixCommonRegisters.GetQuickCode(189, "POOR"));
                
                //string Remarks = General.GetNullableString(txtRemarks.Text);
                                
                if (!IsValidbulk(General.GetNullableInteger(btnAnsSearch.SelectedValue), General.GetNullableString(txtRemarks.Text), No, Poor))
                {
                    ucError.Visible = true;
                    return;
                }

                if (Filter.CurrentInspectionChapter != null)
                {
                    ArrayList selectChapter = (ArrayList)Filter.CurrentInspectionChapter;
                    if (selectChapter != null && selectChapter.Count > 0)
                    {
                        foreach (Guid Checkitemid in selectChapter)
                        {
                           

                            PhoenixInspectionAuditInterfaceDetails.InspectionInterfaceCheckItemBulkInsert(
                                                General.GetNullableGuid(ViewState["REVIEWSCHEDULEID"].ToString())
                                                ,General.GetNullableInteger(ViewState["QUESTIONTYPE"].ToString())
                                                ,General.GetNullableString(txtRemarks.Text)
                                                ,General.GetNullableInteger(btnAnsSearch.SelectedValue.ToString())
                                                ,new Guid(Checkitemid.ToString())
                                               );                            
                        }
                        Filter.CurrentInspectionChapter = null;
                        Filter.CurrentInspectionVesselCheckItem = null;
                    }
                    
                }
                
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                              "BookMarkScript", "fnReloadList('Bulk', 'ifMoreInfo', null);", true);

        }
        catch (Exception ex)
        {
            Filter.CurrentInspectionVesselCheckItem = null;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidbulk(int? Answer,string Remarks,int? No,int? Poor)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Answer == null)
            ucError.ErrorMessage = "Answer is required.";

        if ((Remarks == null) && (( Answer == No) || (Answer == Poor)))
            ucError.ErrorMessage = "Remark is required.";

        return (!ucError.IsError);
    }


}