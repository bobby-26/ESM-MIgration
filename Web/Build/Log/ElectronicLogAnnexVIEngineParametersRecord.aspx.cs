using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;
using System.Collections.Specialized;

public partial class Log_ElectronicLogAnnexVIEngineParametersRecord : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["PARAMETRTID"] = Request.QueryString["Parameterid"];
            ViewState["ENGINE"] = Server.UrlDecode(Request.QueryString["Engine"]);
            ViewState["ID"] = Request.QueryString["id"];
            Title = "MARPOL NOx Technical Code " + ViewState["ENGINE"].ToString() + " Parameters";

            if (ViewState["ID"] != null)
            {
                BindData();
                trreason.Visible = true;
            }
        }
        ShowToolBar();
    }

    private void BindData()
    {
        Guid? Id = General.GetNullableGuid(ViewState["ID"].ToString());
        DataTable dt = PhoenixMarpolLogEngineParameters.EngineParameterRecordEdit(Id);
        tbadjustment.Text = dt.Rows[0]["FLDADJUSTMENT"].ToString();
        tbremarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
        tbdate.Text = dt.Rows[0]["FLDDATE"].ToString();
        tbdate.Enabled = false;
        sno.Text = dt.Rows[0]["FLDSNO"].ToString();
        status.Text = dt.Rows[0]["FLDSTATUS"].ToString();
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);


        Tabstrip.MenuList = toolbar.Show();
        Tabstrip.AccessRights = this.ViewState;
    }


    protected void Tabstrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string Adj = General.GetNullableString(tbadjustment.Text);
                string Remarks = General.GetNullableString(tbremarks.Text);
                DateTime? Date = General.GetNullableDateTime(tbdate.Text);
                Guid? ParameterId = General.GetNullableGuid(ViewState["PARAMETRTID"].ToString());
                Guid? ID = Guid.Empty;
                
                int? SNo = General.GetNullableInteger(sno.Text);
                int? Status = General.GetNullableInteger(status.Text);
                if (!IsValidDetails(Adj, Date,tbreason.Text,lblincsign.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ID"] != null)
                {
                    ID = General.GetNullableGuid(ViewState["ID"].ToString());
                    PhoenixMarpolLogEngineParameters.EngineParameterRecordUpdate(ParameterId, Adj, Remarks, Date ,Status , SNo,ID,tbreason.Text, lblinchName.Text, lblincRank.Text);
                }
                else
                {
                    PhoenixMarpolLogEngineParameters.EngineParameterRecordInsert(ParameterId, Adj, Remarks, Date , lblinchName.Text , lblincRank.Text);

                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "closeTelerikWindow('code1', 'Log', false);", true);
            }
        }
        catch (Exception ex)
        { ucError.ErrorMessage = ex.Message; ucError.Visible = true; }
    }

    private bool IsValidDetails(string Adj, DateTime? Date ,string Reason, string Sign)
    {

        ucError.HeaderMessage = "Provide the following required information";


        if (string.IsNullOrEmpty(Adj))
        {
            ucError.ErrorMessage = "Adjustment Made .";
        }
        if (Date == null)
        {
            ucError.ErrorMessage = "Date.";
        }
       if(Date != null && Date > DateTime.Now)
        {
            ucError.ErrorMessage = "Date cannot be greater than today.";
        }

        if (ViewState["ID"] != null)
        {
            if (string.IsNullOrEmpty(Reason))
            {
                ucError.ErrorMessage = "Reason .";
            }
        }
        
        if (string.IsNullOrEmpty(Sign))
        {
            ucError.ErrorMessage = "Sign  .";
        }
        return (!ucError.IsError);
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        string scriptpopup = "";
        string Rank = PhoenixMarpolLogODS.GetRankName(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        


        scriptpopup = String.Format("javascript:parent.openNewWindow('engineersign','','Log/ElectricLogOfficerSignature.aspx?popuptitle=" + "Officer in Charge Signature" + "&rankName=" + Rank + "&LogBookId=" + "" + "&popupname=code1" + "','false','400','190');");
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Filter.DutyEngineerSignatureFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.DutyEngineerSignatureFilterCriteria;


            string Name = nvc.Get("name");
            string Rank = nvc.Get("rank");

            lblinchName.Text = Name;
            lblincRank.Text = Rank;
            lblincsign.Text = "1";
            btnInchargeSign.Visible = false;
            lblinchName.Visible = true;
           
            Filter.DutyEngineerSignatureFilterCriteria = null;

        }
    }
}