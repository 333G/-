/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2017/4/22 14:49:54                           */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.tmp_SMC_SendSms')
            and   type = 'U')
   drop table dbo.tmp_SMC_SendSms
go

execute sp_rename SMC_SendSms, tmp_SMC_SendSms
go

/*==============================================================*/
/* Table: SMC_SendSms                                           */
/*==============================================================*/
create table dbo.SMC_SendSms (
   F_Id                 varchar(50)          collate Chinese_PRC_CI_AS not null,
   F_MobileList         text                 collate Chinese_PRC_CI_AS null,
   F_MobileCount        int                  null,
   F_SmsContent         varchar(500)         collate Chinese_PRC_CI_AS null,
   F_IsTimer            bit                  null constraint DF_SMC_SendSms_Is_Timer default (0),
   F_SendTime           datetime             null,
   F_SendSign           varchar(32)          collate Chinese_PRC_CI_AS null,
   F_SmbSign            varchar(20)          collate Chinese_PRC_CI_AS null,
   F_SendState          int                  null,
   F_DealState          int                  null,
   F_OperateState       int                  null,
   F_Price              decimal(18,2)        null,
   F_GroupChannelId     varchar(50)          null,
   F_RootId             int                  null,
   F_DeleteMark         bit                  null,
   F_EnabledMark        bit                  null,
   F_CreatorTime        datetime             null constraint DF_SMC_SendSms_F_CreatorTime default getdate(),
   F_CreatorUserId      varchar(50)          collate Chinese_PRC_CI_AS null,
   F_LastModifyTime     datetime             null,
   F_LastModifyUserId   varchar(50)          collate Chinese_PRC_CI_AS null,
   F_DeleteTime         datetime             null,
   F_DeleteUserId       varchar(50)          collate Chinese_PRC_CI_AS null,
   constraint PK__ocm_SendSms primary key (F_Id)
         on "PRIMARY"
)
on "PRIMARY"
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('dbo.SMC_SendSms') and minor_id = 0)
begin 
   execute sp_dropextendedproperty 'MS_Description',  
   'user', 'dbo', 'table', 'SMC_SendSms' 
 
end 


execute sp_addextendedproperty 'MS_Description',  
   '发送短信表', 
   'user', 'dbo', 'table', 'SMC_SendSms'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_Id')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_Id'

end


execute sp_addextendedproperty 'MS_Description', 
   '编号',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_MobileList')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_MobileList'

end


execute sp_addextendedproperty 'MS_Description', 
   '收件人号码字符串，分隔符‘，’',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_MobileList'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_MobileCount')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_MobileCount'

end


execute sp_addextendedproperty 'MS_Description', 
   '短信号码数量',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_MobileCount'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_SmsContent')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SmsContent'

end


execute sp_addextendedproperty 'MS_Description', 
   '短信内容',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SmsContent'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_IsTimer')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_IsTimer'

end


execute sp_addextendedproperty 'MS_Description', 
   '是否定时（默认是false,false=及时发送；true=定时发送）',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_IsTimer'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_SendTime')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SendTime'

end


execute sp_addextendedproperty 'MS_Description', 
   '发送时间，用于定时发送',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SendTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_SendSign')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SendSign'

end


execute sp_addextendedproperty 'MS_Description', 
   '短信标记中文',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SendSign'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_SmbSign')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SmbSign'

end


execute sp_addextendedproperty 'MS_Description', 
   '短信标记英文',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SmbSign'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_SendState')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SendState'

end


execute sp_addextendedproperty 'MS_Description', 
   '发送状态(0=待发送；9=发送成功；-1=发送失败；)',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_SendState'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_DealState')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DealState'

end


execute sp_addextendedproperty 'MS_Description', 
   '审核状态(0=未审核；9=已审核；-1=待审核；-2=未通过)',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DealState'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_OperateState')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_OperateState'

end


execute sp_addextendedproperty 'MS_Description', 
   '处理状态(0=待处理；9=已处理；1=处理中)',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_OperateState'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_Price')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_Price'

end


execute sp_addextendedproperty 'MS_Description', 
   '费用',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_Price'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_GroupChannelId')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_GroupChannelId'

end


execute sp_addextendedproperty 'MS_Description', 
   '发送通道',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_GroupChannelId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_RootId')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_RootId'

end


execute sp_addextendedproperty 'MS_Description', 
   '祖宗ID',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_RootId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_DeleteMark')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DeleteMark'

end


execute sp_addextendedproperty 'MS_Description', 
   '删除标记',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DeleteMark'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_EnabledMark')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_EnabledMark'

end


execute sp_addextendedproperty 'MS_Description', 
   '可用标记',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_EnabledMark'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_CreatorTime')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_CreatorTime'

end


execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_CreatorTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_CreatorUserId')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_CreatorUserId'

end


execute sp_addextendedproperty 'MS_Description', 
   '创建人',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_CreatorUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_LastModifyTime')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_LastModifyTime'

end


execute sp_addextendedproperty 'MS_Description', 
   '最后编辑时间',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_LastModifyTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_LastModifyUserId')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_LastModifyUserId'

end


execute sp_addextendedproperty 'MS_Description', 
   '最后编辑人',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_LastModifyUserId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_DeleteTime')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DeleteTime'

end


execute sp_addextendedproperty 'MS_Description', 
   '删除时间',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DeleteTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.SMC_SendSms')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'F_DeleteUserId')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DeleteUserId'

end


execute sp_addextendedproperty 'MS_Description', 
   '删除人',
   'user', 'dbo', 'table', 'SMC_SendSms', 'column', 'F_DeleteUserId'
go

insert into dbo.SMC_SendSms (F_Id, F_MobileList, F_MobileCount, F_SmsContent, F_SendTime, F_SendSign, F_SmbSign, F_SendState, F_DealState, F_Price, F_GroupChannelId, F_RootId, F_DeleteMark, F_EnabledMark, F_CreatorTime, F_CreatorUserId, F_LastModifyTime, F_LastModifyUserId, F_DeleteTime, F_DeleteUserId)
select F_Id, F_MobileList, F_MobileCount, F_SmsContent, F_SendTime, F_SendSign, F_SmbSign, F_SendState, F_DealState, F_Price, F_GroupChannelId, F_RootId, F_DeleteMark, F_EnabledMark, F_CreatorTime, F_CreatorUserId, F_LastModifyTime, F_LastModifyUserId, F_DeleteTime, F_DeleteUserId
from dbo.tmp_SMC_SendSms
go

alter table Sev_SendDateDetail
   add constraint FK_SEV_SEND_REFERENCE_SMC_SEND foreign key (SMC_F_Id)
      references dbo.SMC_SendSms (F_Id)
go

