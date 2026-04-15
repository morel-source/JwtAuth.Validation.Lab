
### JwtSecurity.Lab
A distributed .NET lab demonstrating secure JWT authentication and TCP-based gateway validation.

---

####  🏗️ Architecture
- **Validator.Server:** A high-concurrency gateway using `TcpListener` and `System.Threading.Channels` for non-blocking token validation.
- **Simulator:** A multi-threaded client simulating both legitimate traffic (valid HMAC-signed tokens) and security threats (incorrect cryptographic signatures).

--- 

#### 🛠️ Key Technical Features
- **Asynchronous I/O:** Efficient `TcpClient`/`TcpListener` stream handling.
- **Decoupling:** Uses `Channels` to separate network ingestion from validation logic.
- **Resilience:** Robust error handling that identifies and logs unauthorized signature attempts.
- **Scalability:** Runs concurrent device simulations using `Task.WhenAll`.

---

Built with .NET 10 | Clean Architecture | Async-First Design
