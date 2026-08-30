using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ween.Migrations
{
    /// <inheritdoc />
    public partial class Baseline : Migration
    {
        // Baseline for the pre-existing (database-first) schema. The tables already
        // exist in the database, so Up/Down are intentionally no-ops. This migration
        // exists only to record the current schema in the model snapshot, so the next
        // migration (AddIdentity) diffs against it instead of trying to recreate every table.

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
