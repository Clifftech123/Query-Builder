import axios from 'axios';

const BASE_URL =
  ' https://localhost:7100/api/';

// const BASE_URL = 'https://localhost:7180/api/v1';

// Create axios instance with base configuration
export const api = axios.create({
  baseURL: BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor - Add auth token and handle FormData
api.interceptors.request.use(
  config => {
    const token = localStorage.getItem('accessToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    // If the data is FormData, remove the Content-Type header
    // so axios will set it with the correct boundary
    if (config.data instanceof FormData) {
      delete config.headers['Content-Type'];
    }

    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

// Response interceptor - Handle token refresh
api.interceptors.response.use(
  response => response,
  async error => {
    const originalRequest = error.config;

    // If 401 and we haven't tried to refresh yet
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      const refreshToken = localStorage.getItem('refreshToken');
      const accessToken = localStorage.getItem('accessToken');

      console.log('🔄 Token refresh attempt:', {
        hasRefreshToken: !!refreshToken,
        hasAccessToken: !!accessToken,
        refreshTokenLength: refreshToken?.length || 0,
        accessTokenLength: accessToken?.length || 0,
      });

      // If no tokens exist or they're empty, don't try to refresh
      if (
        !refreshToken ||
        !accessToken ||
        refreshToken.trim() === '' ||
        accessToken.trim() === ''
      ) {
        console.log('❌ No valid tokens found, skipping refresh');
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        // Don't redirect here, let the component handle it
        return Promise.reject(error);
      }

      console.log('✅ Valid tokens found, attempting refresh...');

      try {
        // Try to refresh the token - backend requires both tokens
        // IMPORTANT: Backend expects "expiredAccessToken" not "accessToken"
        console.log('📤 Sending refresh request with tokens:', {
          refreshTokenLength: refreshToken.length,
          accessTokenLength: accessToken.length,
          accessTokenFirst50: accessToken.substring(0, 50),
        });

        const response = await axios.post(
          `${api.defaults.baseURL}/auth/refresh-token`,
          {
            refreshToken: refreshToken,
            expiredAccessToken: accessToken,
          }
        );

        const { token: newAccessToken, refreshToken: newRefreshToken } =
          response.data.data;

        // Update stored tokens
        localStorage.setItem('accessToken', newAccessToken);
        if (newRefreshToken) {
          localStorage.setItem('refreshToken', newRefreshToken);
        }

        // Retry original request with new token
        originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
        return api(originalRequest);
      } catch (refreshError) {
        // Refresh failed, clear tokens and redirect to login
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        window.location.href = '/login';
        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  }
);