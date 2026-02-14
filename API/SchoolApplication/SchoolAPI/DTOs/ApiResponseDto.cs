namespace SchoolAPI.DTOs
{
    /// <summary>
    /// Standardized API response format for all endpoints.
    /// </summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    public class ApiResponseDto<T>
    {
        /// <summary>
        /// HTTP status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Human-readable message describing the response.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The actual data being returned. Nullable for error responses.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Optional error code for better error handling by client.
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Timestamp of when the response was generated.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Paginated response format for list endpoints.
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated list.</typeparam>
    public class PaginatedResponseDto<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public PaginationMetadata Pagination { get; set; } = new();
        public IEnumerable<T>? Items { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Metadata for paginated responses.
    /// </summary>
    public class PaginationMetadata
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}

