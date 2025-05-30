using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniAccountSystem.Models;
using MiniAccountSystem.Models.VoucherModels;
using System.Data;

namespace MiniAccountSystem.Services
{
    public class VoucherService
    {
        private readonly AppDbContext _context;

        public VoucherService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> SaveVoucherAsync(Voucher voucher)
        {
            if (voucher == null) throw new ArgumentNullException(nameof(voucher));

            var voucherTypeParam = new SqlParameter("@VoucherType", voucher.VoucherType);
            var voucherDateParam = new SqlParameter("@VoucherDate", voucher.VoucherDate);
            var referenceNoParam = new SqlParameter("@ReferenceNo", voucher.ReferenceNo ?? (object)DBNull.Value);
            var createdByParam = new SqlParameter("@CreatedBy", voucher.CreatedBy);

            //entries to DataTable for TVP
            var entriesTable = new DataTable();
            entriesTable.Columns.Add("AccountID", typeof(int));
            entriesTable.Columns.Add("DebitAmount", typeof(decimal));
            entriesTable.Columns.Add("CreditAmount", typeof(decimal));
            entriesTable.Columns.Add("Description", typeof(string));

            foreach (var entry in voucher.Entries)
            {
                entriesTable.Rows.Add(entry.AccountID, entry.DebitAmount, entry.CreditAmount, entry.Description);
            }

            var entriesParam = new SqlParameter("@Entries", entriesTable)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.VoucherEntryType"
            };

            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_SaveVoucher @VoucherType, @VoucherDate, @ReferenceNo, @CreatedBy, @Entries",
                voucherTypeParam, voucherDateParam, referenceNoParam, createdByParam, entriesParam);

            return result;
        }
    }
}
